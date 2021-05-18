using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("search.all")]
    public class SearchResponseAll : Response
    {
        public string query { get; set; }
        public SearchResponseMessagesContainer messages { get; set; }
        public SearchResponseFilesContainer files { get; set; }
    }
}