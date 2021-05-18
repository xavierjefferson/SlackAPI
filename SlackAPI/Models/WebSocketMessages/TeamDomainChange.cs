using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("team_domain_change")]
    public class TeamDomainChange
    {
        public string url { get; set; }
        public string domain { get; set; }
    }
}