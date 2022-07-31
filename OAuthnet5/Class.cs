using System.ComponentModel.DataAnnotations;

namespace OAuthnet5
{
    public class GoogleAccessToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string id_token { get; set; }

        public string refresh_token { get; set; }

    }

    public class GoogleCode
    {
        public string state { get; set; }
        public string code { get; set; }
        public string scope { get; set; }
        public string authuser { get; set; }
        public string prompt { get; set; }
    }

    public class GoogleUserOutputData

    {

        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string email { get; set; }

        public string picture { get; set; }

    }

}
