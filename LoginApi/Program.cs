//using Microsoft.AspNetCore.Mvc;
//using TestWebApi.Model;
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using System.Security.Claims;
//using System.IdentityModel.Tokens.Jwt;
//using System;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var devCorsPolicy = "devCorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(devCorsPolicy, builder => {
        //builder.WithOrigins("http://localhost:4200/").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        //builder.SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddSingleton<TokenService>(new TokenService());
builder.Services.AddSingleton<IUserRepositoryService>(new UserRepositoryService());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});



builder.Services.AddAuthorization();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(devCorsPolicy);
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/login", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/login", [AllowAnonymous] async ([FromBodyAttribute] LoginModel userModel, TokenService tokenService, IUserRepositoryService userRepositoryService, HttpResponse response) => {

    var userDto = userRepositoryService.GetUser(userModel);
    if (userDto == null)
    {
        response.StatusCode = 401;
        return;
    }
    var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"], builder.Configuration["Jwt:Issuer"],null, userDto);
    await response.WriteAsJsonAsync(new { token = token });
    return;

}).Produces(StatusCodes.Status200OK)
.WithName("Login").WithTags("Accounts");


app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public record UserDto(string UserName, string Password, string Role);

public record LoginModel
{
       public int Id { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }       
}

public interface ITokenService
{
    string BuildToken(string key, string issuer, string audience, UserDto user);
}

public class TokenService : ITokenService
{
    private TimeSpan ExpiryDuration = new TimeSpan(0, 30, 0);
    public string BuildToken(string key, string issuer, string audience, UserDto loginModel)
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MynameisJamesBond007"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[] {
                new Claim(ClaimTypes.Role, loginModel.Role),
              new Claim(JwtRegisteredClaimNames.UniqueName, loginModel.UserName)
              };
        var token = new JwtSecurityToken(
            issuer: "https://www.yogihosting.com",
            audience: "https://www.yogihosting.com",
            expires: DateTime.Now.AddSeconds(60),
            signingCredentials: credentials,
            claims: claims
                        );


//        var claims = new[]
//        {
//new Claim(ClaimTypes.Name, user.UserName),
//new Claim(ClaimTypes.NameIdentifier,
//Guid.NewGuid().ToString())
//};
//        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
//        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
//        var tokenDescriptor = new JwtSecurityToken(issuer, audience, claims,
//        expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
public interface IUserRepositoryService
{
    UserDto GetUser(LoginModel userModel);
}

public class UserRepositoryService : IUserRepositoryService
{
    private List<UserDto> _users => new()
    {
        new("admin", "abc123","Dev"),
    };

    public UserDto GetUser(LoginModel userModel)
    {
        return _users.FirstOrDefault(x => string.Equals(x.UserName, userModel.Username) && string.Equals(x.Password, userModel.Password));
    }
}

//internal string GenerateJSONWebToken(LoginModel loginModel)
//{
//    // //Token ==Algo + Key 
//    // //key - private key to protect to decrypt// stored in encrypted in DB (Securelocation)
//    // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:SecretKey"]));

//    // //Algo Hmac256
//    // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

//    // //Claims- role or what the user all about // authroization
//    // var claims = new[]
//    // {
//    //     new Claim("Issuer", _configuration["JwtToken:Issuer"]),
//    //     new Claim(ClaimTypes.Role,role),
//    //     new Claim(JwtRegisteredClaimNames.UniqueName,userName)
//    // };

//    // // if u want to token using bearer format should :Authorization:bearer:token

//    // //Issuer, issueed to ,roles,expireing ,username
//    // var token = new JwtSecurityToken(
//    //    _configuration["JwtToken:Issuer"],
//    //    _configuration["JwtToken:Audience"],
//    //     claims,
//    //     expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtToken:TokenExpiry"])),
//    //     signingCredentials: credentials);

//    // // generate token is authentication part

//    // return new JwtSecurityTokenHandler().WriteToken(token);

//var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MynameisJamesBond007"));
//var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//var claims = new[] {
//                new Claim(ClaimTypes.Role, loginModel.Role),
//              new Claim(JwtRegisteredClaimNames.UniqueName, loginModel.Username)
//              };
//var token = new JwtSecurityToken(
//    issuer: "https://www.yogihosting.com",
//    audience: "https://www.yogihosting.com",
//    expires: DateTime.Now.AddSeconds(60),
//    signingCredentials: credentials,
//    claims: claims
//                );
//return new JwtSecurityTokenHandler().WriteToken(token);
//}

//[AllowAnonymous]
//[HttpPost]
//public IActionResult CreateToken([FromBody] LoginModel login)
//{
//    if (login == null) return Unauthorized();
//    string accessToken = string.Empty;
//    var validUser = Authenticate(login);
//    if (validUser != null)
//    {
//        accessToken = GenerateJSONWebToken(validUser);
//    }
//    else
//    {
//        return Unauthorized();
//    }
//    return Ok(new { Token = accessToken });
//}

//private LoginModel Authenticate(LoginModel login)
//{
//    var users = _dBContextDal.Logins;
//    foreach (var user in users)
//    {
//        if (user.Username == login.Username && user.Password == login.Password)
//        {
//            return user;
//        }
//    }
//    return null;
//}