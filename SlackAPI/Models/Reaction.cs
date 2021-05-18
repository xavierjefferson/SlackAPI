using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class Reaction
    {
        public string name { get; set; }
        public int count { get; set; }
        public List<string> users { get; set; }
    }
}