using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("files.list")]
    public class FileListResponse : Response
    {
        public List<File> files {get; set;} = new List<File>();
        public PaginationInformation paging { get; set; }
    }
}