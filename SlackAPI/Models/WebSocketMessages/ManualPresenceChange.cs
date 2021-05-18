using SlackAPI.Attributes;

namespace SlackAPI.Models.WebSocketMessages
{
    [SlackSocketRouting("manual_presence_change")]
    public class ManualPresenceChange : PresenceChange
    {
    }
}