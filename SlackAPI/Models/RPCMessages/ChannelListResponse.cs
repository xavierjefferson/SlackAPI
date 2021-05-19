using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.list")]
    public class ChannelListResponse : Response
    {
        public List<Channel> channels {get; set;} = new List<Channel>();
    }
}