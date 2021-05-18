using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_public")]
    public class FilePublic
    {
        public File file { get; set; }
    }
}