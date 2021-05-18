using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.setTopic")]
    public class ChannelSetTopicResponse : Response
    {
        public string topic { get; set; }
    }
}