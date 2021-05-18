namespace SlackAPI.Models
{
    public class ExternalSelectElement : IElement
    {
        public string type { get; } = ElementTypes.ExternalSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public string initial_option { get; set; }
        public int min_query_length { get; set; }
        public Confirm confirm { get; set; }
    }
}