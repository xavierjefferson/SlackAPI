namespace SlackAPI.Models
{
    public class TeamPreferences
    {
        public enum AuthMode
        {
            normal,
            saml,
            google
        }

        public bool? stats_only_admins;

        public string[] default_channels { get; set; }
        public AuthMode auth_mode { get; set; }
        public bool display_real_names { get; set; }
        public bool gateway_allow_irc_plain { get; set; }
        public bool gateway_allow_irc_ssl { get; set; }
        public bool gateway_allow_xmpp_ssl { get; set; }
        public bool hide_referers { get; set; }
        public int msg_edit_window_mins { get; set; }
        public bool srvices_only_admins { get; set; }
    }
}