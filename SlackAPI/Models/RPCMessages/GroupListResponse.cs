using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.list")]
    public class GroupListResponse : Response
    {
        public List<Channel> groups {get; set;} = new List<Channel>();
    }
}