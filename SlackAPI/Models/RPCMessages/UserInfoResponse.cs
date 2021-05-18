using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.info")]
    public class UserInfoResponse : Response
    {
        public User user { get; set; }
    }
}