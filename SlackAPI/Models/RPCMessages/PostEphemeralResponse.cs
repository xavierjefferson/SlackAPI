using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("chat.postEphemeral")]
    public class PostEphemeralResponse : Response
    {
        public string message_ts { get; set; }
    }
}