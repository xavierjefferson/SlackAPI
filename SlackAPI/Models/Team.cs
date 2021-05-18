using System;

namespace SlackAPI.Models
{
    public class Team
    {
        /// <summary>
        ///     Supported domains emails can be registered from.
        /// </summary>
        public string[] email_domains { get; set; }

        public bool over_storage_limit { get; set; }
        public bool sso { get; set; }
        public SSOProvider[] sso_provider { get; set; }
        public string domain { get; set; }

        /// <summary>
        ///     Supported domains emails can be registered from.
        /// </summary>
        /// TODO: Is this obsolete?
        public string email_domain { get; set; }

        public string id { get; set; }
        public long limit_ts { get; set; }
        public DateTime LimitTimestamp => new DateTime(1970, 1, 1).AddMilliseconds(limit_ts);
        public int msg_edit_window_mins { get; set; }
        public string name { get; set; }
        public TeamPreferences prefs { get; set; }
        public string sso_required { get; set; }
        public string sso_type { get; set; }
        public string url { get; set; }
    }
}