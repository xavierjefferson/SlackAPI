using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.create")]
    public class ConversationsCreateResponse : Response
    {
        public Channel channel { get; set; }
    }
}