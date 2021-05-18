using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("files.list")]
    public class FileListResponse : Response
    {
        public File[] files { get; set; }
        public PaginationInformation paging { get; set; }
    }
}