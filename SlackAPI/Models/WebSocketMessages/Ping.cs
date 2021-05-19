﻿using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("ping")]
    public class Ping : SlackSocketMessage
    {
        public int ping_interv_ms = 3000;
    }
}