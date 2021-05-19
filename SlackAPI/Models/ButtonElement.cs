using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class ButtonElement : IElement
    {
        public string type { get; } = ElementTypes.Button;
        public string action_id { get; set; }
        public Text text { get; set; }
        public string value { get; set; }
        public Text placeholder { get; set; }
        public List<Option> options {get; set;} = new List<Option>();
        public List<OptionGroups> option_groups {get; set;} = new List<OptionGroups>();
        public string url { get; set; }
        public Confirm confirm { get; set; }
        public string style { get; set; }
    }
}