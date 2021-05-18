using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_unarchive")]
    public class GroupUnarchive : SlackSocketMessage
    {
        public string channel { get; set; }
    }
}