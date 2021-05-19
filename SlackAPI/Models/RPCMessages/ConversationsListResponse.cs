using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.list")]
    public class ConversationsListResponse : Response
    {
        public List<Channel> channels {get; set;} = new List<Channel>();
    }
}