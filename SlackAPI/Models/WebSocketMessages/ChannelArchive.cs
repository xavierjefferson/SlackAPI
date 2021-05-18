using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_archive")]
    public class ChannelArchive
    {
        public string channel { get; set; }
        public string user { get; set; }
    }
}