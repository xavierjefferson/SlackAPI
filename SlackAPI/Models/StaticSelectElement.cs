namespace SlackAPI.Models
{
    public class StaticSelectElement : IElement
    {
        public string type { get; } = ElementTypes.StaticSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public Option[] options { get; set; }
        public OptionGroups[] option_groups { get; set; }
        public string initial_option { get; set; }
        public Confirm confirm { get; set; }
    }
}