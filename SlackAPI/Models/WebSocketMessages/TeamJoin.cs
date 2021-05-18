using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("team_join")]
    public class TeamJoin : SlackSocketMessage
    {
        public User user { get; set; }
    }
}