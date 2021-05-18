namespace SlackAPI.Models.RPCMessages
{
    public class Star
    {
        public string type { get; set; }
        public string channel { get; set; }
        public Message message { get; set; }
        public string file { get; set; }
        public FileComment comment { get; set; }
    }
}