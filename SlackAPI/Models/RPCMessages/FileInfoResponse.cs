using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("files.info")]
    public class FileInfoResponse : Response
    {
        public File file { get; set; }
        public FileComment[] comments { get; set; }
        public PaginationInformation paging { get; set; }
    }
}