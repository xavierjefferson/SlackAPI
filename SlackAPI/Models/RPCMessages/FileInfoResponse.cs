using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("files.info")]
    public class FileInfoResponse : Response
    {
        public File file { get; set; }
        public List<FileComment> comments {get; set;} = new List<FileComment>();
        public PaginationInformation paging { get; set; }
    }
}