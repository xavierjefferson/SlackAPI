using System;
using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("message")]
    [SlackSocketRouting("message", "bot_message")]
    public class NewMessage : SlackSocketMessage
    {
        public NewMessage()
        {
            type = "message";
        }

        public string user { get; set; }
        public string channel { get; set; }
        public string text { get; set; }
        public string team { get; set; }
        public DateTime ts { get; set; }
        public DateTime thread_ts { get; set; }
        public string username { get; set; }
        public string bot_id { get; set; }
        public UserProfile icons { get; set; }
        public List<Block> blocks { get; set; }
        public List<Attachment> attachments { get; set; }
    }
}