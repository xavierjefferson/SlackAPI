using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_joined")]
    public class GroupJoined : SlackSocketMessage
    {
        public Channel channel { get; set; }
    }
}