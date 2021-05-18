using System;

namespace SlackAPI.Models
{
    public class FileComment
    {
        public string comment { get; set; }
        public string id { get; set; }
        public DateTime timestamp { get; set; }
        public string user { get; set; }
    }
}