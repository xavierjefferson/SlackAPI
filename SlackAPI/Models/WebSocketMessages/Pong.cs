using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("pong")]
    public class Pong : SlackSocketMessage
    {
        public int ping_interv_ms { get; set; }
    }
}