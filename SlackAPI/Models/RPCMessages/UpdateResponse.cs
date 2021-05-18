using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("chat.update")]
    public class UpdateResponse : Response
    {
        public string channel { get; set; }
        public string ts { get; set; }
        public string text { get; set; }
        public Message message { get; set; }

        public class Message
        {
            public string type { get; set; }
            public string user { get; set; }
            public string text { get; set; }
        }
    }
}