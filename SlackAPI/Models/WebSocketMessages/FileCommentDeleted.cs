using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_comment_deleted")]
    public class FileCommentDeleted
    {
        public File file { get; set; }
        public string comment { get; set; }
    }
}