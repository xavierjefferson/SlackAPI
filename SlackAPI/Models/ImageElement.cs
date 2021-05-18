namespace SlackAPI.Models
{
    public class ImageElement : IElement
    {
        public string type { get; } = ElementTypes.Image;
        public string image_url { get; set; }
        public string alt_text { get; set; }
    }
}