using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_left")]
    public class GroupLeft : SlackSocketMessage
    {
        public string channel { get; set; }
    }
}