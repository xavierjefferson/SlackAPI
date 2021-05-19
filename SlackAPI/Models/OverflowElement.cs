using System.Collections.Generic;

namespace SlackAPI.Models
{
    public class OverflowElement : IElement
    {
        public string type { get; } = ElementTypes.Overflow;
        public string action_id { get; set; }
        public List<Option> options {get; set;} = new List<Option>();
        public Confirm confirm { get; set; }
    }
}