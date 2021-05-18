using System;

namespace SlackAPI.Models
{
    [Flags]
    public enum FileTypes
    {
        all = 63,
        posts = 1,
        snippets = 2,
        images = 4,
        gdocs = 8,
        zips = 16,
        pdfs = 32
    }
}