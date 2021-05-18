using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.invite")]
    public class ChannelInviteResponse : Response
    {
        public Channel channel { get; set; }
    }
}