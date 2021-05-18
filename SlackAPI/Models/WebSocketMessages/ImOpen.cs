using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("im_open")]
    public class ImOpen
    {
        public string user { get; set; }
        public string channel { get; set; }
    }
}