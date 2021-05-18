using System;
using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("channel_marked")]
    public class ChannelMarked : SlackSocketMessage
    {
        public string channel { get; set; }
        public DateTime ts { get; set; }
    }
}