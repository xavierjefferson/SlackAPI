using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_history_changed")]
    public class GroupHistoryChanged
    {
        public string latest { get; set; }
        public string ts { get; set; }
        public string event_ts { get; set; }
    }
}