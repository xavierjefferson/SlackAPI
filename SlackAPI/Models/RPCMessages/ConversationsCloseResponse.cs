using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.close")]
    public class ConversationsCloseResponse : Response
    {
        public string no_op { get; set; }
        public string already_closed { get; set; }
    }
}