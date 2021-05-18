using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.setPurpose")]
    public class GroupSetPurposeResponse : Response
    {
        public string purpose { get; set; }
    }
}