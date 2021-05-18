using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("search.messages")]
    public class SearchResponseMessages : Response
    {
        public string query { get; set; }
    }
}