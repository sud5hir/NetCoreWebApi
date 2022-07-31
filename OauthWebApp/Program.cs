using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/account/google-login"; // Must be lowercase
}).AddGoogle(options =>
{
    options.ClientId = "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

app.MapPost("/payments", (PaymentRequest paymentRequest) =>
{

});
