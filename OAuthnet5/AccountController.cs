using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OAuthnet5;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OauthWebApp
{
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    {

        //        https://accounts.google.com/o/oauth2/auth?client_id="957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
        //&redirect_uri=="https://localhost:44377/signin-google"&scope="https://www.googleapis.com/auth/cloud-platform.read-only"&response_type="code"

        //https://accounts.google.com/o/oauth2/v2/auth? 
        //scope=https://www.googleapis.com/auth/gmail.metadata+https://www.googleapis.com/auth/gmail.readonly+https://www.googleapis.com/auth/drive+https://www.googleapis.com/auth/gmail.readonly+https://www.googleapis.com/auth/userinfo.profile
        //&response_type=code
        // &state=state_parameter_passthrough_value
        // &redirect_uri=https://localhost:44377/signin-google
        //&client_id=957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com

        //https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount
        //?response_type=code
        //&client_id=957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
        //&redirect_uri=https%3A%2F%2Flocalhost%3A44377%2Fsignin-googl
        //e&scope=openid%20profile%20email
        //&state=CfDJ8IUFg-QaEXdCi7XH8AKPdUCGSHwvAjPLcJt6EN0t1lz6xzbjsg4Cn7fU9W8eELpumxssJijPYvUOiHHXIVU34RtT8xWjfQtqBs3OF_Fyq2cNvcSewc9HJUJxBm8NOtt6ANDb8VPepswsdSfm7IyNiXp-XN8AIdW-GXXqjzzOmbPCYjZ2sEdSVf3kVFYvRJ9SsEgqvE8jcpPlmzr29M5UePNjINMYebrbUxIPFkNwOwDi
        //&flowName=GeneralOAuthFlow

        //public IActionResult Index()
        //{
        //    return View("");
        //}
        public IActionResult GoogleLogin()
        {
            var Parameters = "https://accounts.google.com/o/oauth2/v2/auth?" + "&redirect_uri=" + "https://localhost:44377/Account/signin-google"
                + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
                + "&scope=" + "openid" + "+profile" + "+email"
                + "&response_type=" + "code"
                + "&access_type=" + "offline"
                + "&prompt=" + "consent";
                 

            //var RedirectUri = Url.Action("signin-google", "Account",
            //new { ReturnUri = Parameters }, "https");

            return new RedirectResult(Parameters);


            //GetTokenFromGoogle();

            //var properties = new AuthenticationProperties { RedirectUri = Parameters, };
            //return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("signin-google")]
        public async Task<IActionResult> GoogleResponse(string code, string scope, string authuser, string prompt)
        {
            var url = Request.Query;

            var tokenResponse = await ExchangeAuthorizationCode(code,
                "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com",
                "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", "https://localhost:44377/Account/signin-google");

            var refreshTokenResponse = await ExchangeRefreshToken(tokenResponse.refresh_token, "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
                , "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");

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            //GetTokenFromGoogle();
            var userData = getgoogleplususerdataSer(tokenResponse.access_token).Result;

            var lis = new System.Collections.Generic.Dictionary<string, string>();
            lis.Add("AccessToken", tokenResponse.access_token);
            lis.Add("RefreshToken", tokenResponse.refresh_token);
            lis.Add("userData", userData.given_name);


            return Json(lis);
        }

        public static async Task<GoogleAccessToken> ExchangeAuthorizationCode(string code, string clientId, string secret,
           string redirectUri = null)
        {
            var result = new GoogleAccessToken();

            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = "urn:ietf:wg:oauth:2.0:oob"; // for installed application
            }

            var postData = BuildAuthorizationCodeRequest(code, clientId, secret, redirectUri);

            return await PostMessage(postData);
        }

        private static string BuildAuthorizationCodeRequest(string code, string clientId, string secret,
            string redirectUri)
        {
            return
                $"code={code}&client_id={clientId}&client_secret={secret}&redirect_uri={redirectUri}&grant_type=authorization_code";
        }

        private static async Task<GoogleAccessToken> PostMessage(string postData)
        {
            GoogleAccessToken result;

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://oauth2.googleapis.com");
            var request = new HttpRequestMessage(HttpMethod.Post, "/token");
            request.Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);
            using (var content = response.Content)
            {
                var json = content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<GoogleAccessToken>(json);
            }
            return result;
        }

        private async Task<GoogleUserOutputData> getgoogleplususerdataSer(string access_token)
        {
            GoogleUserOutputData serStatus= null;
            try
            {
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);

                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                     serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                        // You will get the user information here.
                    }
                }
              
            }
            catch (Exception ex)
            {
                //catching the exception
            }
            return serStatus;
        }

        //public class GoogleUserOutputData
        //{
        //    public string id { get; set; }
        //    public string name { get; set; }
        //    public string given_name { get; set; }
        //    public string email { get; set; }
        //    public string picture { get; set; }
        //}
        private static string BuildRefreshAccessTokenRequest(string refreshToken, string clientId, string secret)
        {
            return
                $"client_id={clientId}&client_secret={secret}&refresh_token={refreshToken}&grant_type=refresh_token";
        }

        public static async Task<GoogleAccessToken> ExchangeRefreshToken(string refreshToken, string clientId, string secret)
        {
            var postData = BuildRefreshAccessTokenRequest(refreshToken, clientId, secret);

            return await PostMessage(postData);
        }

      
        private void GetTokenFromGoogle()
        {
            // GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
            // {
            //     ClientSecrets = new ClientSecrets
            //     {
            //         ClientId = "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com",
            //         ClientSecret = "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"
            //     }
            // };
            // initializer.DataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);
            // GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

            //// Console.WriteLine("Will load the saved access token");
            // TokenResponse token = flow.LoadTokenAsync("user", CancellationToken.None).Result;


            //get the access token

            var t = Url.Action("GoogleResponse");
            //  GET https://accounts.google.com/o/oauth2/v2/auth?client_id={clientid}&redirect_uri={RedirectURI}&scope={scopes}&response_type=code

            //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/v2/auth");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");

            webRequest.Method = "POST";
            var a = "https://localhost:44377/signin-google";

            //var Parameters = "&redirect_uri=" + "https://localhost:44377/signin-google"
            //      + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
            //      + "&scope=" + "openid" + "+profile" + "+email"
            //      + "&response_type=" + "code" ;

            //        https://accounts.google.com/o/oauth2/v2/auth?
            //            &redirect_uri = https://localhost:44377/signin-google
            //&client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
            //    & scope = openid + profile + email
            //    & response_type = code

            //https://localhost:44377/signin-google
            //? code = 4 % 2F0AX4XfWgUEQ4lU745yhrxWVwUvAL6vK6yr57xm80C3LE4dLojpoA3DsmZQohU7GVZRoZ2YQ
            //     & scope = email + profile + openid + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.profile + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.email
            //                                    & authuser = 0
            //                                    & prompt = consent

            //var Parameters = "code=" + "4%2F0AX4XfWghQL59bHgXrwY-ySdayiYPTRVxwW-iFIf38t8BuAhRpOPk7PEWwWfQf6foC0UH7w"
            //     + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
            //     +"&client_secret=" + "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" + "&redirect_uri=" + "https://localhost:44377/signin-google" 
            //     + "&grant_type=authorization_code";

            var Parameters = "&redirect_uri=" + "https://localhost:44377/signin-google"
                  + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
                  + "&scope=" + "openid" + "+profile" + "+email"
                  + "&response_type=" + "code";



            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);


            GoogleAccessToken result;

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://accounts.google.com/");
            var request = new HttpRequestMessage(HttpMethod.Post, "o/oauth2/auth");
            request.Content = new StringContent(Parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response1 = client.SendAsync(request).Result;
            using (var content = response1.Content)
            {
                var json = content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<GoogleAccessToken>(json);
            }

            webRequest.ContentType = "application/x-www-form-urlencoded";

            webRequest.ContentLength = byteArray.Length;

            Stream postStream = webRequest.GetRequestStream();

            // Add the post data to the web request

            postStream.Write(byteArray, 0, byteArray.Length);

            postStream.Close();

            WebResponse response = webRequest.GetResponse();

            postStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(postStream);

            string responseFromServer = reader.ReadToEnd();

            GoogleAccessToken serStatus = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);

            if (serStatus != null)

            {

                string accessToken = string.Empty;

                //    accessToken = serStatus.access_token;

                //Session["Token"] = accessToken;

                if (!string.IsNullOrEmpty(accessToken))

                {

                    //call get user information function with access token as parameter

                }

            }
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //public IActionResult Login(string returnUrl)
        //{
        //    Login login = new Login();
        //    login.ReturnUrl = returnUrl;
        //    return View(login);
        //}

        //[AllowAnonymous]
        //public IActionResult GoogleLogin1()
        //{
        //    string redirectUrl = Url.Action("GoogleResponse1", "Account");
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        //    return new ChallengeResult("Google", properties);
        //}





        //[AllowAnonymous]
        //public async Task<IActionResult> GoogleResponse1()
        //{
        //    ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null)
        //        return RedirectToAction(nameof(Login));

        //    var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //    string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
        //    if (result.Succeeded)
        //        return View(userInfo);
        //    else
        //    {
        //        AppUser user = new AppUser
        //        {
        //            Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //            UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
        //        };

        //        IdentityResult identResult = await _userManager.CreateAsync(user);
        //        if (identResult.Succeeded)
        //        {
        //            identResult = await _userManager.AddLoginAsync(user, info);
        //            if (identResult.Succeeded)
        //            {
        //                await _signInManager.SignInAsync(user, false);
        //                return View(userInfo);
        //            }
        //        }
        //        return View(userInfo); 
        //    }
        //}

    }
}
