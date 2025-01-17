﻿using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("chat.postMessage")]
    public class PostMessageResponse : Response
    {
        public string ts { get; set; }
        public string channel { get; set; }
        public Message message { get; set; }

        public class Message
        {
            public string text { get; set; }
            public string user { get; set; }
            public string username { get; set; }
            public string type { get; set; }
            public string subtype { get; set; }
            public string ts { get; set; }
        }
    }
}