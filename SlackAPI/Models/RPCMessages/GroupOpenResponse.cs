using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.open")]
    public class GroupOpenResponse : Response
    {
        public string no_op { get; set; }
        public string already_closed { get; set; }
    }
}