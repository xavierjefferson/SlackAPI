namespace SlackAPI.Models.RPCMessages
{
    public class PaginationInformation
    {
        public int count { get; set; }
        public int total { get; set; }
        public int page { get; set; }
        public int pages { get; set; }

        //These are defined for search results?  Undocumented stuff ftw? :P
        /// <summary>
        ///     Undocumented. Use with care.
        /// </summary>
        public int first { get; set; }

        /// <summary>
        ///     Undocumented. Use with care.
        /// </summary>
        public int last { get; set; }

        /// <summary>
        ///     Undocumented. Use with care.
        /// </summary>
        public int page_count { get; set; }

        /// <summary>
        ///     Undocumented. Use with care.
        /// </summary>
        public int per_page { get; set; }

        /// <summary>
        ///     Undocumented. Use with care.
        /// </summary>
        public int total_count { get; set; }
    }
}