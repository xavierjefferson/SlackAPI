using System;

namespace SlackAPI.Models
{
    //TODO: See below:
    /// <summary>
    ///     Please do a full, thorough, review of this.
    /// </summary>
    public class File
    {
        public string[] channels { get; set; }
        public int comments_count { get; set; }
        public DateTime created { get; set; }
        public bool display_as_bot { get; set; }
        public string edit_link { get; set; }
        public bool editable { get; set; }
        public string external_type { get; set; }
        public string filetype { get; set; }
        public string[] groups { get; set; }
        public string id { get; set; }
        public string[] ims { get; set; }
        public FileComment initial_comment { get; set; }
        public bool is_external { get; set; }

        public bool is_public { get; set; }
        public bool is_starred { get; set; }
        public int lines { get; set; }
        public int lines_more { get; set; }
        public string mimetype { get; set; }

        public string mode { get; set; }

        public string name { get; set; }
        public int num_stars { get; set; }

        public string permalink { get; set; }
        public string permalink_public { get; set; }
        public string[] pinned_to { get; set; }
        public string pretty_type { get; set; }
        public string preview { get; set; }
        public string preview_highlight { get; set; }
        public bool public_url_shared { get; set; }

        public Reaction[] reactions { get; set; }

        /// <summary>
        ///     File size in bytes
        /// </summary>
        public int size { get; set; }

        public string thumb_160 { get; set; }
        public string thumb_360 { get; set; }
        public string thumb_360_gif { get; set; }
        public int thumb_360_h { get; set; }
        public int thumb_360_w { get; set; }
        public string thumb_480 { get; set; }
        public int thumb_480_h { get; set; }
        public int thumb_480_w { get; set; }

        public string thumb_64 { get; set; }
        public string thumb_80 { get; set; }
        public DateTime timestamp { get; set; }
        public string title { get; set; }

        public string url { get; set; }
        public string url_download { get; set; }
        public string url_private { get; set; }
        public string url_private_download { get; set; }
        public string user { get; set; }
        public string username { get; set; }
    }
}