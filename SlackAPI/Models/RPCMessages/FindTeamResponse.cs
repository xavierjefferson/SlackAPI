using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    /// <summary>
    ///     This is an undocumented response from an undocumented API. If anyone finds more info on this, please create a pull
    ///     request.
    /// </summary>
    [RequestPath("auth.findTeam")]
    public class FindTeamResponse : Response
    {
        public string sso_required { get; set; }
        public string sso_type { get; set; }
        public string team_id { get; set; }
        public string url { get; set; }
        public List<string> email_domains {get; set;} = new List<string>();
        public bool sso { get; set; }
        public List<SSOProvider> sso_provider {get; set;} = new List<SSOProvider>();

        public static implicit operator Team(FindTeamResponse resp)
        {
            var end = new Team
            {
                sso_required = resp.sso_required,
                sso_type = resp.sso_type,
                id = resp.team_id,
                url = resp.url,
                sso = resp.sso,
                email_domains = resp.email_domains,
                sso_provider = resp.sso_provider
            };
            return end;
        }
    }
}