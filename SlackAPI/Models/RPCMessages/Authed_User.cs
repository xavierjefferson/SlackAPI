namespace SlackAPI.Models.RPCMessages
{
    public class Authed_User
    {
        public string id { get; set; }

        public string scope { get; set; }

        public string access_token { get; set; }

        public string token_type { get; set; }
    }
}