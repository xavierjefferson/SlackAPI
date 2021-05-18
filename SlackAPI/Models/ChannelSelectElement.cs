namespace SlackAPI.Models
{
    public class ChannelSelectElement : IElement
    {
        public string type { get; } = ElementTypes.ChannelSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public string initial_channel { get; set; }
        public Confirm confirm { get; set; }
    }
}