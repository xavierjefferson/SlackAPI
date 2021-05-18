namespace SlackAPI.Models
{
    //See: https://api.slack.com/docs/attachments
    public class Attachment
    {
        public AttachmentAction[] actions { get; set; }
        public string author_icon { get; set; }
        public string author_link { get; set; }
        public string author_name { get; set; }
        public IBlock[] blocks { get; set; }
        public string callback_id { get; set; }
        public string color { get; set; }
        public string fallback { get; set; }
        public Field[] fields { get; set; }

        public string footer { get; set; }
        public string footer_icon { get; set; }

        public string image_url { get; set; }
        public string[] mrkdwn_in { get; set; }
        public string pretext { get; set; }
        public string text { get; set; }
        public string thumb_url { get; set; }
        public string title { get; set; }
        public string title_link { get; set; }
    }

    //See: https://api.slack.com/docs/message-buttons#action_fields

    //see: https://api.slack.com/docs/message-buttons#confirmation_fields
}