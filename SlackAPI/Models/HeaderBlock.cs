namespace SlackAPI.Models
{
    public class HeaderBlock : IBlock
    {
        public string type { get; } = BlockTypes.Header;
        public Text text { get; set; }
        public string block_id { get; set; }
    }
}