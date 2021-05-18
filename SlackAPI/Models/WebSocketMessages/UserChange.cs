using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("user_change")]
    public class UserChange : SlackSocketMessage
    {
        public User user { get; set; }
    }
}