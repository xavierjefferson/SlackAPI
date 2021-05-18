using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_unarchive")]
    public class ChannelUnarchive
    {
        public string channel { get; set; }
        public string user { get; set; }
    }
}