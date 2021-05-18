using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.getPresence")]
    public class UserGetPresenceResponse : Response
    {
        public Presence presence { get; set; }
    }
}