﻿using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("conversations.open")]
    public class JoinDirectMessageChannelResponse : Response
    {
        public Channel channel { get; set; }
    }
}