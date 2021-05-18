using System;
using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("message", "message_deleted")]
    public class DeletedMessage : SlackSocketMessage
    {
        public string channel { get; set; }
        public DateTime ts { get; set; }
        public DateTime deleted_ts { get; set; }
        public bool hidden { get; set; }
    }
}