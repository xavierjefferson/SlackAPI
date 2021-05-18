namespace SlackAPI.Models
{
    public class UserSelectElement : IElement
    {
        public string type { get; } = ElementTypes.UserSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public string initial_user { get; set; }
        public Confirm confirm { get; set; }
    }
}