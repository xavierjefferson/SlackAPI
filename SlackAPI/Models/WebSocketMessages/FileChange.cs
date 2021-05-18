using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_change")]
    public class FileChange
    {
        public File file { get; set; }
    }
}