namespace SlackAPI.Models
{
    public class Element : IElement
    {
        public string type { get; set; }
        public string action_id { get; set; }
        public Text text { get; set; }
        public string value { get; set; }
        public Text placeholder { get; set; }
        public Option[] options { get; set; }
        public OptionGroups[] option_groups { get; set; }
        public string image_url { get; set; }
        public string alt_text { get; set; }
        public string url { get; set; }
        public string initial_date { get; set; }
        public string initial_user { get; set; }
        public string initial_channel { get; set; }
        public string initial_conversation { get; set; }
        public string initial_option { get; set; }
        public int? min_query_length { get; set; }
        public Confirm confirm { get; set; }
        public string style { get; set; }
    }
}