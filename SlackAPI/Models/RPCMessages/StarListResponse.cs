using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("stars.list")]
    public class StarListResponse : Response
    {
        public Star[] items { get; set; }
        public PaginationInformation paging { get; set; }
    }
}