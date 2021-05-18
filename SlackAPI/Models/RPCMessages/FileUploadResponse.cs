using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("files.upload")]
    public class FileUploadResponse : Response
    {
        public File file { get; set; }
    }
}