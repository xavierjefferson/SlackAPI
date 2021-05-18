using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.close")]
    public class GroupCloseResponse : Response
    {
        public string no_op { get; set; }
        public string already_closed { get; set; }
    }
}