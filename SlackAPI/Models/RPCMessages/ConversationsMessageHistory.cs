using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.history")]
    public class ConversationsMessageHistory : MessageHistory
    {
    }
}