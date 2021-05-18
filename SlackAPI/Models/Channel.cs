namespace SlackAPI.Models
{
    public class Channel : Conversation
    {
        public string creator { get; set; }

        public bool is_archived { get; set; }
        public bool is_channel { get; set; }
        public bool is_general { get; set; }
        public bool is_group { get; set; }
        public bool is_im { get; set; }
        public bool is_member { get; set; }

        public string[] members { get; set; }
        public string name { get; set; }

        public int num_members { get; set; }
        public OwnedStampedMessage purpose { get; set; }
        public OwnedStampedMessage topic { get; set; }

        public string user { get; set; }

        //Is this deprecated by is_open?
        public bool IsPrivateGroup => id != null && id[0] == 'G';
    }
}