using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.list")]
    public class GroupListResponse : Response
    {
        public Channel[] groups { get; set; }
    }
}