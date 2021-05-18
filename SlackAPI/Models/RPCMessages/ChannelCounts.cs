namespace SlackAPI.Models.RPCMessages
{
    public class ChannelCounts
    {
        public string id { get; set; }
        public int mention_count { get; set; }
        public string name { get; set; }
        public int unread_count { get; set; }
    }
}