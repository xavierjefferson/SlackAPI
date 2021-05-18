namespace SlackAPI.Models
{
    public class SlackSocketMessage
    {
        public bool ok = true;
        public int id { get; set; }
        public int reply_to { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public Error error { get; set; }
    }
}