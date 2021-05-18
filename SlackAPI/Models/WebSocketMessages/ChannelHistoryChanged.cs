using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_history_changed")]
    public class ChannelHistoryChanged
    {
        public string latest { get; set; }
        public string ts { get; set; }
        public string event_ts { get; set; }
    }
}