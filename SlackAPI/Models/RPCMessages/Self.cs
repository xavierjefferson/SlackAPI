using System;

namespace SlackAPI.Models.RPCMessages
{
    public class Self
    {
        public DateTime created { get; set; }
        public string id { get; set; }
        public string manual_presence { get; set; }
        public string name { get; set; }
        public Preferences prefs { get; set; }
    }
}