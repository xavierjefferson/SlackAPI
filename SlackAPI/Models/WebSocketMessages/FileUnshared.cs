using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_unshared")]
    public class FileUnshared
    {
        public File file { get; set; }
    }
}