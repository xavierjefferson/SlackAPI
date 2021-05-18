using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("users.prefs.get")]
    public class UserPreferencesResponse : Response
    {
        public Preferences prefs { get; set; }
    }
}