namespace SlackAPI.Models.RPCMessages
{
    public class SearchResponseFilesContainer
    {
        /// <summary>
        ///     Can be null if used in the context of search.all
        ///     Please use paging.total instead.
        /// </summary>
        public int total { get; set; }

        public PaginationInformation paging { get; set; }
        public File[] matches { get; set; }
    }
}