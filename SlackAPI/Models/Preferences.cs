﻿namespace SlackAPI.Models
{
    public class Preferences
    {
        public bool email_misc { get; set; }
        public bool push_everything { get; set; }
        public bool seen_notification_prefs_banner { get; set; }
        public bool seen_welcome_2 { get; set; }
        public bool seen_user_menu_tip_card { get; set; }
        public bool seen_message_input_tip_card { get; set; }
        public bool seen_channels_tip_card { get; set; }
        public bool seen_team_menu_tip_card { get; set; }
        public bool seen_flexpane_tip_card { get; set; }
        public bool seen_search_input_tip_card { get; set; }
        public bool has_uploaded { get; set; }
        public bool search_only_my_channels { get; set; }
        public bool seen_channel_menu_tip_card { get; set; }
        public bool has_invited { get; set; }
        public bool has_created_channel { get; set; }
        public bool color_names_in_list { get; set; }
        public bool growls_enabled { get; set; }
        public bool push_dm_alert { get; set; }
        public bool push_mention_alert { get; set; }
        public bool welcome_message_hidden { get; set; }
        public bool all_channels_loud { get; set; }
        public bool show_member_presence { get; set; }
        public bool expand_inline_imgs { get; set; }
        public bool expand_internal_inline_imgs { get; set; }
        public bool seen_ssb_prompt { get; set; }
        public bool webapp_spellcheck { get; set; }
        public bool no_joined_overlays { get; set; }
        public bool no_created_overlays { get; set; }
        public bool dropbox_enabled { get; set; }
        public bool mute_sounds { get; set; }
        public bool arrow_history { get; set; }
        public bool tab_ui_return_selects { get; set; }
        public bool obey_inline_img_limit { get; set; }
        public bool collapsible { get; set; }
        public bool collapsible_by_click { get; set; }
        public bool require_at { get; set; }
        public bool mac_ssb_bullet { get; set; }
        public bool expand_non_media_attachments { get; set; }
        public bool show_typing { get; set; }
        public bool pagekeys_handled { get; set; }
        public bool time24 { get; set; }
        public bool enter_is_special_in_tbt { get; set; }
        public bool graphic_emoticons { get; set; }
        public bool convert_emoticons { get; set; }
        public bool autoplay_chat_sounds { get; set; }
        public bool ss_emojis { get; set; }
        public bool mark_msgs_read_immediately { get; set; }
        public string tz { get; set; }
        public string emoji_mode { get; set; }

        public string highlight_words { get; set; }

        //public string newxp_slackbot_step{get;set;} //I don't even...
        public string search_sort { get; set; }
        public string push_loud_channels { get; set; }
        public string push_mention_channels { get; set; }
        public string push_loud_channels_set { get; set; }
        public string user_colors { get; set; }
        public int push_idle_wait { get; set; }
        public string push_sound { get; set; }
        public string email_alerts { get; set; }
        public int email_alerts_sleep_until { get; set; }
        public string loud_channels { get; set; }
        public string never_channels { get; set; }
        public string loud_channels_set { get; set; }
        public string search_excluse_channels { get; set; }
        public string messages_theme { get; set; }
        public string new_msg_snd { get; set; }
        public string mac_ssb_bounce { get; set; }
        public string last_snippet_type { get; set; }
        public int display_real_names_override { get; set; }
        public string muted_channels { get; set; }
    }
}