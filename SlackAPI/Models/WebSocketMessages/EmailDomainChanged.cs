using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("email_domain_changed")]
    public class EmailDomainChanged
    {
        public string email_domain { get; set; }
        public string event_ts { get; set; }
    }
}