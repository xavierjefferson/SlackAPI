using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_deleted")]
    public class ChannelDeleted
    {
        public string channel { get; set; }
    }
}