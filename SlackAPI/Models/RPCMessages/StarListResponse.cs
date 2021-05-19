using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("stars.list")]
    public class StarListResponse : Response
    {
        public List<Star> items {get; set;} = new List<Star>();
        public PaginationInformation paging { get; set; }
    }
}