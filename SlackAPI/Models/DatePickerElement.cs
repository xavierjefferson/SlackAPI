namespace SlackAPI.Models
{
    public class DatePickerElement : IElement
    {
        public string type { get; } = ElementTypes.DatePicker;
        public string action_id { get; set; }
        public Text placeholder { get; set; }
        public string initial_date { get; set; }
        public Confirm confirm { get; set; }
    }
}