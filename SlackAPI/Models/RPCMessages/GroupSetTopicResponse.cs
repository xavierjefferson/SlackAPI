using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.setTopic")]
    public class GroupSetTopicResponse : Response
    {
        public string topic { get; set; }
    }
}