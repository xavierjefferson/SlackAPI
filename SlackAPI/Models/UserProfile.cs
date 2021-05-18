using SlackAPI.Models;

namespace SlackAPI
{
    public class UserProfile : ProfileIcons
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string real_name { get; set; }
        public string email { get; set; }
        public string skype { get; set; }
        public string status_emoji { get; set; }
        public string status_text { get; set; }
        public string phone { get; set; }

        public override string ToString()
        {
            return real_name;
        }
    }
}