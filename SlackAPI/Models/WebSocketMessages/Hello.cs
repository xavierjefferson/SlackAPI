using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("hello")]
    public class Hello : SlackSocketMessage
    {
    }
}