using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("file_deleted")]
    public class FileDeleted
    {
        public string file_id { get; set; }
        public string event_ts { get; set; }
    }
}