using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("reaction_added")]
    public class ReactionAdded : SlackSocketMessage
    {
        public string user { get; set; }
        public string reaction { get; set; }
        public string item_user { get; set; }
        public Item item { get; set; }
        public string event_ts { get; set; }
    }
}