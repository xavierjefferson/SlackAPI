using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("rtm.start")]
    public class LoginResponse : Response
    {
        public Bot[] bots { get; set; }
        public Channel[] channels { get; set; }
        public Channel[] groups { get; set; }
        public DirectMessageConversation[] ims { get; set; }
        public Self self { get; set; }
        public int svn_rev { get; set; }
        public int min_svn_rev { get; set; }
        public Team team { get; set; }
        public string url { get; set; }
        public User[] users { get; set; }
    }
}