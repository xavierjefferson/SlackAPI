using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("im.list")]
    public class DirectMessageConversationListResponse : Response
    {
        public List<DirectMessageConversation> ims {get; set;} = new List<DirectMessageConversation>();
    }
}