namespace SlackAPI.Models
{
    public class Bot
    {
        public string id { get; set; }
        public bool deleted { get; set; }
        public string name { get; set; }
        public string updated { get; set; }
        public string app_id { get; set; }
        public ProfileIcons icons { get; set; }
    }
}