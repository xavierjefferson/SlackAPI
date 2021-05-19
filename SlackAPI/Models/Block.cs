using System.Collections.Generic;

namespace SlackAPI.Models
{
    //see https://api.slack.com/reference/messaging/blocks
    public class Block : IBlock
    {
        public string type { get; set; }
        public string block_id { get; set; }
        public Text text { get; set; }
        public Element accessory { get; set; }
        public List<Element> elements {get; set;} = new List<Element>();
        public Text title { get; set; }
        public string image_url { get; set; }
        public string alt_text { get; set; }
        public List<Text> fields {get; set;} = new List<Text>();
    }
}