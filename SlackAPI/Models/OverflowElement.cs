namespace SlackAPI.Models
{
    public class OverflowElement : IElement
    {
        public string type { get; } = ElementTypes.Overflow;
        public string action_id { get; set; }
        public Option[] options { get; set; }
        public Confirm confirm { get; set; }
    }
}