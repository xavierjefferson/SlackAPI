using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.list")]
    public class ConversationsListResponse : Response
    {
        public Channel[] channels { get; set; }
    }
}