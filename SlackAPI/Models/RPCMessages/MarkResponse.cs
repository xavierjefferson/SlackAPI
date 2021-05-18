using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    /// <summary>
    ///     This is used for moving the read cursor in the channel.
    /// </summary>
    [RequestPath("channels.mark")]
    public class MarkResponse : Response
    {
    }
}