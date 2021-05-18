namespace SlackAPI.Models
{
    public class Text : IElement
    {
        public string type { get; set; } = TextTypes.PlainText;
        public string text { get; set; }
        public bool? emoji { get; set; }
        public bool? verbatim { get; set; }
    }
}