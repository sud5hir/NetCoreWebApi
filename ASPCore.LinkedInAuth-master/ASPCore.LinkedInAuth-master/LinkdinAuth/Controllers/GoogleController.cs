using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LinkdinAuth.Controllers
{
    public class GoogleController : Controller
    {
        public IActionResult Index()
        {
            // var Parameters = "https://accounts.google.com/o/oauth2/v2/auth?" + "&redirect_uri=" + "https://localhost:44377/signin-google"
            //    + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
            //    + "&scope=" + "openid" + "+profile" + "+email"
            //    + "&response_type=" + "code";


            //return Redirect(Parameters);


            //GetTokenFromGoogle();

            var properties = new AuthenticationProperties { RedirectUri = Url.Action("google-response") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }


        [Route("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var url = Request.Query;

            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            //var res=GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets(
            ////   {
            ////    ClientId= "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com",
            ////    ClientSecret="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"
            ////},)).Result;

            //GetTokenFromGoogle();

            return Json(claims);

        }
    }
}

//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Auth.OAuth2.Flows;
//using Google.Apis.Auth.OAuth2.Responses;
//using Google.Apis.Util.Store;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using OAuthnet5;
//using System;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace OauthWebApp
//{
//    [AllowAnonymous, Route("account")]
//    public class AccountController : Controller
//    {

//        //        https://accounts.google.com/o/oauth2/auth?client_id="957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
//        //&redirect_uri=="https://localhost:44377/signin-google"&scope="https://www.googleapis.com/auth/cloud-platform.read-only"&response_type="code"

//        //https://accounts.google.com/o/oauth2/v2/auth? 
//        //scope=https://www.googleapis.com/auth/gmail.metadata+https://www.googleapis.com/auth/gmail.readonly+https://www.googleapis.com/auth/drive+https://www.googleapis.com/auth/gmail.readonly+https://www.googleapis.com/auth/userinfo.profile
//        //&response_type=code
//        // &state=state_parameter_passthrough_value
//        // &redirect_uri=https://localhost:44377/signin-google
//        //&client_id=957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com

//        //https://accounts.google.com/o/oauth2/v2/auth/oauthchooseaccount
//        //?response_type=code
//        //&client_id=957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//        //&redirect_uri=https%3A%2F%2Flocalhost%3A44377%2Fsignin-googl
//        //e&scope=openid%20profile%20email
//        //&state=CfDJ8IUFg-QaEXdCi7XH8AKPdUCGSHwvAjPLcJt6EN0t1lz6xzbjsg4Cn7fU9W8eELpumxssJijPYvUOiHHXIVU34RtT8xWjfQtqBs3OF_Fyq2cNvcSewc9HJUJxBm8NOtt6ANDb8VPepswsdSfm7IyNiXp-XN8AIdW-GXXqjzzOmbPCYjZ2sEdSVf3kVFYvRJ9SsEgqvE8jcpPlmzr29M5UePNjINMYebrbUxIPFkNwOwDi
//        //&flowName=GeneralOAuthFlow

//        //public IActionResult Index()
//        //{
//        //    return View("");
//        //}
//        public IActionResult GoogleLogin()
//        {
//            //var Parameters = "https://accounts.google.com/o/oauth2/v2/auth?" + "&redirect_uri=" + "https://localhost:44377/signin-google"
//            //    + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
//            //    + "&scope=" + "openid" + "+profile" + "+email"
//            //    + "&response_type=" + "code";


//            //return Redirect(Parameters);


//            //GetTokenFromGoogle();

//            var properties = new AuthenticationProperties { RedirectUri = Url.Action("google-response") };
//            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
//        }

//        [Route("google-response")]
//        public async Task<IActionResult> GoogleResponse()
//        {
//            var url = Request.Query;

//            //if (url != "")

//            //{

//            //    string queryString = url.ToString();

//            //    char[] delimiterChars = { '=' };

//            //    string[] words = queryString.Split(delimiterChars);

//            //    string code = words[1];


//            //}

//            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

//            var claims = result.Principal.Identities
//                .FirstOrDefault().Claims.Select(claim => new
//                {
//                    claim.Issuer,
//                    claim.OriginalIssuer,
//                    claim.Type,
//                    claim.Value
//                });

//            //var res=GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets(
//            ////   {
//            ////    ClientId= "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com",
//            ////    ClientSecret="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"
//            ////},)).Result;

//            //GetTokenFromGoogle();

//            return Json(claims);
//        }

//        private void GetTokenFromGoogle()
//        {
//            // GoogleAuthorizationCodeFlow.Initializer initializer = new GoogleAuthorizationCodeFlow.Initializer
//            // {
//            //     ClientSecrets = new ClientSecrets
//            //     {
//            //         ClientId = "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com",
//            //         ClientSecret = "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"
//            //     }
//            // };
//            // initializer.DataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);
//            // GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow(initializer);

//            //// Console.WriteLine("Will load the saved access token");
//            // TokenResponse token = flow.LoadTokenAsync("user", CancellationToken.None).Result;


//            //get the access token

//            var t = Url.Action("GoogleResponse");
//            //  GET https://accounts.google.com/o/oauth2/v2/auth?client_id={clientid}&redirect_uri={RedirectURI}&scope={scopes}&response_type=code

//            //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/v2/auth");
//            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");

//            webRequest.Method = "POST";
//            var a = "https://localhost:44377/signin-google";

//            //var Parameters = "&redirect_uri=" + "https://localhost:44377/signin-google"
//            //      + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
//            //      + "&scope=" + "openid" + "+profile" + "+email"
//            //      + "&response_type=" + "code" ;

//            //        https://accounts.google.com/o/oauth2/v2/auth?
//            //            &redirect_uri = https://localhost:44377/signin-google
//            //&client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//            //    & scope = openid + profile + email
//            //    & response_type = code

//            //https://localhost:44377/signin-google
//            //? code = 4 % 2F0AX4XfWgUEQ4lU745yhrxWVwUvAL6vK6yr57xm80C3LE4dLojpoA3DsmZQohU7GVZRoZ2YQ
//            //     & scope = email + profile + openid + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.profile + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.email
//            //                                    & authuser = 0
//            //                                    & prompt = consent

//            //var Parameters = "code=" + "4%2F0AX4XfWghQL59bHgXrwY-ySdayiYPTRVxwW-iFIf38t8BuAhRpOPk7PEWwWfQf6foC0UH7w"
//            //     + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
//            //     +"&client_secret=" + "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" + "&redirect_uri=" + "https://localhost:44377/signin-google" 
//            //     + "&grant_type=authorization_code";

//            var Parameters = "&redirect_uri=" + "https://localhost:44377/signin-google"
//                  + "&client_id=" + "957350596571-2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com"
//                  + "&scope=" + "openid" + "+profile" + "+email"
//                  + "&response_type=" + "code";


//            byte[] byteArray = Encoding.UTF8.GetBytes(Parameters);


//            GoogleAccessToken result;

//            var client = new HttpClient();
//            client.BaseAddress = new Uri("https://accounts.google.com/");
//            var request = new HttpRequestMessage(HttpMethod.Post, "o/oauth2/auth");
//            request.Content = new StringContent(Parameters, Encoding.UTF8, "application/x-www-form-urlencoded");
//            var response1 = client.SendAsync(request).Result;
//            using (var content = response1.Content)
//            {
//                var json = content.ReadAsStringAsync().Result;
//                result = JsonConvert.DeserializeObject<GoogleAccessToken>(json);
//            }

//            webRequest.ContentType = "application/x-www-form-urlencoded";

//            webRequest.ContentLength = byteArray.Length;

//            Stream postStream = webRequest.GetRequestStream();

//            // Add the post data to the web request

//            postStream.Write(byteArray, 0, byteArray.Length);

//            postStream.Close();

//            WebResponse response = webRequest.GetResponse();

//            postStream = response.GetResponseStream();

//            StreamReader reader = new StreamReader(postStream);

//            string responseFromServer = reader.ReadToEnd();

//            GoogleAccessToken serStatus = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);

//            if (serStatus != null)

//            {

//                string accessToken = string.Empty;

//                //    accessToken = serStatus.access_token;

//                //Session["Token"] = accessToken;

//                if (!string.IsNullOrEmpty(accessToken))

//                {

//                    //call get user information function with access token as parameter

//                }

//            }
//        }

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

//    }
//}
//https://accounts.google.com/o/oauth2/v2/auth?
//&redirect_uri = https://localhost:44377/signin-google
//&client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//    & scope = openid + profile + email
//    & response_type = code

//code = 4 % 2F0AX4XfWiaYgP5HuSt9iXicguUvTWhzQmn1XMMoIivSm_QOK_SLn9XO5K9AzNPr5nqVimDfg
//    & scope = email + profile + openid + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.profile + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.email
//                                   & authuser = 0
//                                   & prompt = consent

//https://accounts.google.com/o/oauth2/v2/auth?
//&redirect_uri =/ account / google - response
//& client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//     & scope = openid + profile + email
//     & response_type = code

//https://localhost:44377/signin-google
//? code = 4 % 2F0AX4XfWgUEQ4lU745yhrxWVwUvAL6vK6yr57xm80C3LE4dLojpoA3DsmZQohU7GVZRoZ2YQ
//     & scope = email + profile + openid + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.profile + https % 3A % 2F % 2Fwww.googleapis.com % 2Fauth % 2Fuserinfo.email
//                                    & authuser = 0
//                                    & prompt = consent


//https://accounts.google.com/o/oauth2/token?
//code = 2F0AX4XfWgUEQ4lU745yhrxWVwUvAL6vK6yr57xm80C3LE4dLojpoA3DsmZQohU7GVZRoZ2YQ
//  & client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//       & client_secret = GOCSPX - SP4nVfb4EaCagt_3SoTy_omNsd5h
//       & redirect_uri = https://localhost:44377/signin-google
//&grant_type = authorization_code

//https://oauth2.googleapis.com/token?
//code = 2F0AX4XfWgUEQ4lU745yhrxWVwUvAL6vK6yr57xm80C3LE4dLojpoA3DsmZQohU7GVZRoZ2YQ
//  & client_id = 957350596571 - 2ll6cm1l0cit5jt5sklfcdnsbv08geb0.apps.googleusercontent.com
//       & client_secret = GOCSPX - SP4nVfb4EaCagt_3SoTy_omNsd5h
//       & redirect_uri = https://localhost:44377/signin-google
//&grant_type = authorization_code

//? client_id ={ client_id}
//&client_secret ={ client_secret}
//&code ={ authorizationcode}
//&redirect_uri ={ redirect_uri}
//&grant_type = authorization_code
