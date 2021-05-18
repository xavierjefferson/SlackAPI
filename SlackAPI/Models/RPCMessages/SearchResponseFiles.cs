using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("search.files")]
    public class SearchResponseFiles : Response
    {
        public string query { get; set; }
        public SearchResponseFilesContainer files { get; set; }
    }
}