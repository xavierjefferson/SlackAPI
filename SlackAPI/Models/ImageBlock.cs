namespace SlackAPI.Models
{
    public class ImageBlock : IBlock
    {
        public string type { get; } = BlockTypes.Image;
        public string block_id { get; set; }
        public Text title { get; set; }
        public string image_url { get; set; }
        public string alt_text { get; set; }
    }
}