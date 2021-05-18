namespace SlackAPI.Models
{
    public class ContextBlock : IBlock
    {
        public string type { get; } = BlockTypes.Context;
        public string block_id { get; set; }
        public IElement[] elements { get; set; }
    }
}