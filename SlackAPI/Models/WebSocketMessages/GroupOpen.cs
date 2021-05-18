using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_open")]
    public class GroupOpen : SlackSocketMessage
    {
        public string user { get; set; }
        public string channel { get; set; }
    }
}