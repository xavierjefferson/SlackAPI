using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.rename")]
    public class GroupRenameResponse : Response
    {
        public Channel channel { get; set; }
    }
}