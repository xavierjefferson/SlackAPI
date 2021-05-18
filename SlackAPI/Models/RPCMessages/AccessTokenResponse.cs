using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("oauth.v2.access")]
    public class AccessTokenResponse : Response
    {
        public string app_id { get; set; }

        public Authed_User authed_user { get; set; }

        public Team1 team { get; set; }

        public object enterprise { get; set; }

        public bool is_enterprise_install { get; set; }
    }
}