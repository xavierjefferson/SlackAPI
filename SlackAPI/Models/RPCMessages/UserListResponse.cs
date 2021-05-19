using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.list")]
    public class UserListResponse : Response
    {
        public List<User> members {get; set;} = new List<User>();
    }
}