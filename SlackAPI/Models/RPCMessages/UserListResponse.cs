using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.list")]
    public class UserListResponse : Response
    {
        public User[] members { get; set; }
    }
}