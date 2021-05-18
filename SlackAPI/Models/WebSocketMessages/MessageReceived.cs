using System;

namespace SlackAPI.Models.WebSocketMessages
{
    public class MessageReceived : SlackSocketMessage
    {
        public string text { get; set; }
        public DateTime ts { get; set; }
    }
}