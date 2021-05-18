using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("im_history_changed")]
    public class ImHistoryChanged
    {
        public string latest { get; set; }
        public string ts { get; set; }
        public string event_ts { get; set; }
    }
}