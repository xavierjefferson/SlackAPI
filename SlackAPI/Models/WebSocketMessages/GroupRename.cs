using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_rename")]
    public class GroupRename : SlackSocketMessage
    {
        public Channel channel { get; set; }
    }
}