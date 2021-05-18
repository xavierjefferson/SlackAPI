using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.setTopic")]
    public class ConversationsSetTopicResponse : Response
    {
        public string topic { get; set; }
    }
}