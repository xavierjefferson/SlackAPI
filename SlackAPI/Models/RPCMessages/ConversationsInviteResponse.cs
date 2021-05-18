using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.invite")]
    public class ConversationsInviteResponse : Response
    {
        public Channel channel { get; set; }
    }
}