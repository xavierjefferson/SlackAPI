using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class ContextBlock : IBlock
    {
        public string type { get; } = BlockTypes.Context;
        public string block_id { get; set; }
        public List<IElement> elements {get; set;} = new List<IElement>();
    }
}