using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.rename")]
    public class ConversationsRenameResponse : Response
    {
        public Channel channel { get; set; }
    }
}