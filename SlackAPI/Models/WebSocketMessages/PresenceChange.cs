using SlackAPI.Attributes;
using SlackAPI.Models.RPCMessages;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("presence_change")]
    public class PresenceChange : SlackSocketMessage
    {
        public string user { get; set; }
        public Presence presence { get; set; }
    }
}