namespace SlackAPI.Models
{
    public class AttachmentAction
    {
        public string type {get;set; } = "button";

        public AttachmentAction(string name, string text)
        {
            this.name = name;
            this.text = text;
        }

        public ActionConfirmEnum confirm { get; set; }
        public string style { get; set; }
        public string url { get; set; }
        public string value { get; set; }

        public string name { get; }
        public string text { get; }
    }
}