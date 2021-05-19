using System.Collections.Generic;

namespace SlackAPI.Models
{
    //See: https://api.slack.com/docs/attachments
    public class Attachment
    {
        public List<AttachmentAction> actions {get; set;} = new List<AttachmentAction>();
        public string author_icon { get; set; }
        public string author_link { get; set; }
        public string author_name { get; set; }
        public List<IBlock> blocks {get; set;} = new List<IBlock>();
        public string callback_id { get; set; }
        public string color { get; set; }
        public string fallback { get; set; }
        public List<Field> fields {get; set;} = new List<Field>();

        public string footer { get; set; }
        public string footer_icon { get; set; }

        public string image_url { get; set; }
        public List<string> mrkdwn_in {get; set;} = new List<string>();
        public string pretext { get; set; }
        public string text { get; set; }
        public string thumb_url { get; set; }
        public string title { get; set; }
        public string title_link { get; set; }
    }

    //See: https://api.slack.com/docs/message-buttons#action_fields

    //see: https://api.slack.com/docs/message-buttons#confirmation_fields
}