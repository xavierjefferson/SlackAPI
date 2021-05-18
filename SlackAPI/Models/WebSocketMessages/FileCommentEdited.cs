using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_comment_edited")]
    public class FileCommentEdited
    {
        public File file { get; set; }
        public string comment { get; set; }
    }
}