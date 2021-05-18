using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("emoji_changed", "add")]
    public class EmojiChangedAdd
    {
        public string name { get; set; }
        public string value { get; set; }
        public string event_ts { get; set; }
    }
}