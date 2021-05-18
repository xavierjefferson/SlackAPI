using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.setPurpose")]
    public class ConversationsSetPurposeResponse : Response
    {
        public string purpose { get; set; }
    }
}