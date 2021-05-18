using System;

namespace SlackAPI
{
    public class User
    {
        public string id { get; set; }

        public bool IsSlackBot => id.Equals("USLACKBOT", StringComparison.CurrentCultureIgnoreCase);

        public string name { get; set; }
        public bool deleted { get; set; }
        public string color { get; set; }
        public UserProfile profile { get; set; }
        public bool is_admin { get; set; }
        public bool is_owner { get; set; }
        public bool is_primary_owner { get; set; }
        public bool is_restricted { get; set; }
        public bool is_ultra_restricted { get; set; }
        public bool has_2fa { get; set; }
        public string two_factor_type { get; set; }
        public bool has_files { get; set; }
        public string presence { get; set; }
        public bool is_bot { get; set; }
        public string tz { get; set; }
        public string tz_label { get; set; }
        public int tz_offset { get; set; }
        public string team_id { get; set; }
        public string real_name { get; set; }
    }
}