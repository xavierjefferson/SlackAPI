using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("chat.scheduleMessage")]
    public class ScheduleMessageResponse : Response
    {
        public string ts { get; set; }
        public string channel { get; set; }
        public string scheduled_message_id { get; set; }
        public int post_at { get; set; }
        public Message message { get; set; }

        public class Message
        {
            public string text { get; set; }
            public string user { get; set; }
            public string username { get; set; }
            public string type { get; set; }
            public string subtype { get; set; }
        }
    }
}