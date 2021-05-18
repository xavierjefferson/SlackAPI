namespace SlackAPI.Models
{
    public class DirectMessageConversation : Conversation
    {
        public string user { get; set; }
        public bool is_user_deleted { get; set; }
    }
}