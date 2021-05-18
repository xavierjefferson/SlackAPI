using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("groups.history")]
    public class GroupMessageHistory : MessageHistory
    {
    }
}