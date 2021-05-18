using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("auth.signin")]
    public class AuthSigninResponse : Response
    {
        public string token { get; set; }
        public string user { get; set; }
        public string team { get; set; }
    }
}