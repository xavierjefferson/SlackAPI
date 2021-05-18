using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.create")]
    public class ChannelCreateResponse : Response
    {
        public Channel channel { get; set; }
    }
}