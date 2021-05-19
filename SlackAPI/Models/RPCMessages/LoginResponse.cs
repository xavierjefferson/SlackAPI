using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("rtm.start")]
    public class LoginResponse : Response
    {
        public List<Bot> bots {get; set;} = new List<Bot>();
        public List<Channel> channels {get; set;} = new List<Channel>();
        public List<Channel> groups {get; set;} = new List<Channel>();
        public List<DirectMessageConversation> ims {get; set;} = new List<DirectMessageConversation>();
        public Self self { get; set; }
        public int svn_rev { get; set; }
        public int min_svn_rev { get; set; }
        public Team team { get; set; }
        public string url { get; set; }
        public List<User> users {get; set;} = new List<User>();
    }
}