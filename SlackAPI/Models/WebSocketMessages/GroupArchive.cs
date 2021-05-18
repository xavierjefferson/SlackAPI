using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("group_archive")]
    public class GroupArchive : SlackSocketMessage
    {
        public string channel { get; set; }
    }
}