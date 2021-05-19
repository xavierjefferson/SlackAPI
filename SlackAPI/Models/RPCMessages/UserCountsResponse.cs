using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.counts")]
    public class UserCountsResponse : Response
    {
        public List<ChannelCounts> channels {get; set;} = new List<ChannelCounts>();
        public List<ChannelCounts> groups {get; set;} = new List<ChannelCounts>();
        public List<DirectMessageNewCount> ims {get; set;} = new List<DirectMessageNewCount>();
    }
}