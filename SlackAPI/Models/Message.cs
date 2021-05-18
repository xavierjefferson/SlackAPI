﻿using System;

namespace SlackAPI.Models
{
    public class Message : SlackSocketMessage
    {
        public string channel { get; set; }
        public DateTime ts { get; set; }
        public string user { get; set; }

        /// <summary>
        ///     Isn't always set. Should look up if not set.
        /// </summary>
        public string username { get; set; }

        public string text { get; set; }
        public bool is_starred { get; set; }
        public string permalink { get; set; }

        public Reaction[] reactions { get; set; }
        //Wibblr? Not really sure what this applies to.  :<

        public DateTime? thread_ts { get; set; }
    }
}