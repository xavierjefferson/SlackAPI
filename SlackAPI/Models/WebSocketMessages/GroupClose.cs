using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_close")]
    public class GroupClose : SlackSocketMessage
    {
        public string user { get; set; }
        public string channel { get; set; }
    }
}