using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("presence_sub")]
    public class PresenceChangeSubscription : SlackSocketMessage
    {
        public PresenceChangeSubscription(string[] usersIds)
        {
            ids = usersIds;
        }

        public string[] ids { get; }
    }
}