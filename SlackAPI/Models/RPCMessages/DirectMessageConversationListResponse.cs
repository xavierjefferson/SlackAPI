using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("im.list")]
    public class DirectMessageConversationListResponse : Response
    {
        public DirectMessageConversation[] ims { get; set; }
    }
}