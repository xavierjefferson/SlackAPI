using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("emoji_changed", "remove")]
    public class EmojiChangedRemove
    {
        public string[] names { get; set; }
        public string event_ts { get; set; }
    }
}