using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_created")]
    public class ChannelCreated
    {
        public Channel channel { get; set; }
    }
}