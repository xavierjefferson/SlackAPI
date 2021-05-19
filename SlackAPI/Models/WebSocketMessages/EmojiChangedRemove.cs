using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("emoji_changed", "remove")]
    public class EmojiChangedRemove
    {
        public List<string> names {get; set;} = new List<string>();
        public string event_ts { get; set; }
    }
}