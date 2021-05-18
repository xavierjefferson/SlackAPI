namespace SlackAPI.Models
{
    public class DividerBlock : IBlock
    {
        public string type { get; } = BlockTypes.Divider;
        public string block_id { get; set; }
    }
}