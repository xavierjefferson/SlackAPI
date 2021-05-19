using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class SectionBlock : IBlock
    {
        public string type { get; } = BlockTypes.Section;
        public string block_id { get; set; }
        public Text text { get; set; }
        public IElement accessory { get; set; }
        public List<Text> fields {get; set;} = new List<Text>();
    }
}