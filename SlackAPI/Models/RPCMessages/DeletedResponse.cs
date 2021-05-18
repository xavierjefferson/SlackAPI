using System;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("chat.delete")]
    public class DeletedResponse : Response
    {
        public string channel { get; set; }
        public DateTime ts { get; set; }
    }
}