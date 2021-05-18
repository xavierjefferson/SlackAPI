using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("typing")]
    [SlackSocketRouting("user_typing")]
    public class Typing : SlackSocketMessage
    {
        public string user { get; set; }
        public string channel { get; set; }
    }
}