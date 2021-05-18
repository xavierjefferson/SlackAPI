namespace SlackAPI.Models
{
    public class ActionsBlock : IBlock
    {
        public string type { get; } = BlockTypes.Actions;
        public string block_id { get; set; }
        public IElement[] elements { get; set; }
    }
}