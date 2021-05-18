using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("message", "file_share")]
    public class FileShareMessage : NewMessage
    {
        public bool upload { get; set; }

        public File file { get; set; }
    }
}