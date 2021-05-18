using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.open")]
    public class ConversationsOpenResponse : Response
    {
        public string no_op { get; set; }
        public string already_open { get; set; }
    }
}