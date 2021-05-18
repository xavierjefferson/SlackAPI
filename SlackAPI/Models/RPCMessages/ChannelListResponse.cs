using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.list")]
    public class ChannelListResponse : Response
    {
        public Channel[] channels { get; set; }
    }
}