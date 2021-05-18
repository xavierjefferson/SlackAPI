using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.lookupByEmail")]
    public class UserEmailLookupResponse : Response
    {
        public User user { get; set; }
    }
}