using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("channels.history")]
    public class ChannelMessageHistory : MessageHistory
    {
    }
}