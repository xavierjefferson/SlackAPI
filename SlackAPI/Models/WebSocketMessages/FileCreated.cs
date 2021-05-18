using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_created")]
    public class FileCreated
    {
        public File file { get; set; }
    }
}