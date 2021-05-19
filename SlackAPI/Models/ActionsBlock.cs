using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class ActionsBlock : IBlock
    {
        public string type { get; } = BlockTypes.Actions;
        public string block_id { get; set; }
        public List<IElement> elements {get; set;} = new List<IElement>();
    }
}