using System;

namespace SlackAPI.Models
{
    public class Conversation
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime last_read { get; set; }
        public bool is_open { get; set; }
        public bool is_starred { get; set; }
        public int unread_count { get; set; }
        public Message latest { get; set; }
    }
}