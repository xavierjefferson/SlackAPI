namespace SlackAPI.Models
{
    public class ConversationSelectElement : IElement
    {
        public string type { get; } = ElementTypes.ChannelSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public string initial_conversation { get; set; }
        public Confirm confirm { get; set; }
    }
}