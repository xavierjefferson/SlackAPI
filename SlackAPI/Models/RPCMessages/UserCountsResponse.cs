using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.counts")]
    public class UserCountsResponse : Response
    {
        public ChannelCounts[] channels { get; set; }
        public ChannelCounts[] groups { get; set; }
        public DirectMessageNewCount[] ims { get; set; }
    }
}