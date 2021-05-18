using System;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("im.history")]
    public class MessageHistory : Response
    {
        /// <summary>
        ///     I believe this is where the read cursor is?  IE: How far the user has read.
        /// </summary>
        public DateTime latest { get; set; }

        public Message[] messages { get; set; }
        public bool has_more { get; set; }
        public int unread_count_display { get; set; }

        public bool channel_not_found { get; set; }
        public bool invalid_ts_latest { get; set; }
        public bool invalid_ts_oldest { get; set; }
    }
}