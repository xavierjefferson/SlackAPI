using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("auth.test")]
    public class AuthTestResponse : Response
    {
        public string url { get; set; }
        public string team { get; set; }
        public string user { get; set; }
        public string team_id { get; set; }
        public string user_id { get; set; }
    }
}