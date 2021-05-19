using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class OptionGroups
    {
        public Text label { get; set; }
        public List<Option> options {get; set;} = new List<Option>();
    }
}