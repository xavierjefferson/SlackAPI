using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("im_close")]
    public class ImClosed
    {
        public string user { get; set; }
        public string channel { get; set; }
    }
}