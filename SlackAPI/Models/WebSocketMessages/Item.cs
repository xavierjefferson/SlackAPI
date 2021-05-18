namespace SlackAPI.Models.WebSocketMessages
{
    public class Item
    {
        public string type { get; set; }
        public string channel { get; set; }
        public string file { get; set; }
        public string file_comment { get; set; }
        public string ts { get; set; }
    }
}