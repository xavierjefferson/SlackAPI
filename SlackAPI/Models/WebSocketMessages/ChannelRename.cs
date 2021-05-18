using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_rename")]
    public class ChannelRename
    {
        public Channel channel { get; set; }
    }
}