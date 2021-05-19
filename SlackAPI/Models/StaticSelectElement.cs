using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class StaticSelectElement : IElement
    {
        public string type { get; } = ElementTypes.StaticSelect;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public List<Option> options {get; set;} = new List<Option>();
        public List<OptionGroups> option_groups {get; set;} = new List<OptionGroups>();
        public string initial_option { get; set; }
        public Confirm confirm { get; set; }
    }
}