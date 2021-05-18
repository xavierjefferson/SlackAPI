namespace SlackAPI.Models
{
    public class ContextMessage : Message
    {
        //public string type {get;set;}
        /// <summary>
        ///     Only contains partial channel data.
        /// </summary>
        //public Channel channel {get;set;}
        //public string user {get;set;}
        //public string username {get;set;}
        //public DateTime ts {get;set;}
        //public string text {get;set;}
        //public string permalink {get;set;}
        public Message previous_2 { get; set; }

        public Message previous { get; set; }
        public Message next { get; set; }
        public Message next_2 { get; set; }
    }
}