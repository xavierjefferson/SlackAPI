namespace SlackAPI.Models
{
    public class ButtonElement : IElement
    {
        public string type { get; } = ElementTypes.Button;
        public string action_id { get; set; }
        public Text text { get; set; }
        public string value { get; set; }
        public Text placeholder { get; set; }
        public Option[] options { get; set; }
        public OptionGroups[] option_groups { get; set; }
        public string url { get; set; }
        public Confirm confirm { get; set; }
        public string style { get; set; }
    }
}