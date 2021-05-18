using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using SlackAPI.Models;
using SlackAPI.Models.RPCMessages;

namespace SlackAPI
{
    /// <summary>
    ///     SlackClient is intended to solely handle RPC (HTTP-based) functionality. Does not handle WebSocket connectivity.
    ///     For WebSocket connectivity, refer to <see cref="SlackAPI.SlackSocketClient" />
    /// </summary>
    public class SlackClient : SlackClientBase
    {
        public List<Bot> Bots {get;set;}
        public Dictionary<string, Channel> ChannelLookup;
        public List<Channel> Channels {get;set;}
        public Dictionary<string, Conversation> ConversationLookup;
        public Dictionary<string, DirectMessageConversation> DirectMessageLookup;
        public List<DirectMessageConversation> DirectMessages {get;set;}
        public Dictionary<string, Channel> GroupLookup;
        public List<Channel> Groups {get;set;}
         public User MyData {get;set;}
 
        public Self MySelf {get;set;}
         public Team MyTeam {get;set;}

        public List<string> starredChannels {get;set;}

        public Dictionary<string, User> UserLookup;

        public List<User> Users {get;set;}

        public SlackClient(string token) : base(token)
        {
        }

        public SlackClient(string token, IWebProxy proxySettings) : base(token, proxySettings)

        {
        }

        public virtual void Connect(Action<LoginResponse> onConnected = null, Action onSocketConnected = null)
        {
            EmitLogin(loginDetails =>
            {
                if (loginDetails.ok)
                    Connected(loginDetails);

                if (onConnected != null)
                    onConnected(loginDetails);
            });
        }

        protected virtual void Connected(LoginResponse loginDetails)
        {
            MySelf = loginDetails.self;
            MyData = loginDetails.users.First(c => c.id == MySelf.id);
            MyTeam = loginDetails.team;

            Users = new List<User>(loginDetails.users.Where(c => !c.deleted));
            Bots = new List<Bot>(loginDetails.bots.Where(c => !c.deleted));
            Channels = new List<Channel>(loginDetails.channels);
            Groups = new List<Channel>(loginDetails.groups);
            DirectMessages =
                new List<DirectMessageConversation>(loginDetails.ims.Where(c =>
                    Users.Exists(a => a.id == c.user) && c.id != MySelf.id));
            starredChannels =
                Groups.Where(c => c.is_starred).Select(c => c.id)
                    .Union(
                        DirectMessages.Where(c => c.is_starred).Select(c => c.user)
                    ).Union(
                        Channels.Where(c => c.is_starred).Select(c => c.id)
                    ).ToList();

            UserLookup = new Dictionary<string, User>();
            foreach (var u in Users) UserLookup.Add(u.id, u);

            ChannelLookup = new Dictionary<string, Channel>();
            ConversationLookup = new Dictionary<string, Conversation>();
            foreach (var c in Channels)
            {
                ChannelLookup.Add(c.id, c);
                ConversationLookup.Add(c.id, c);
            }

            GroupLookup = new Dictionary<string, Channel>();
            foreach (var g in Groups)
            {
                GroupLookup.Add(g.id, g);
                ConversationLookup.Add(g.id, g);
            }

            DirectMessageLookup = new Dictionary<string, DirectMessageConversation>();
            foreach (var im in DirectMessages)
            {
                DirectMessageLookup.Add(im.id, im);
                ConversationLookup.Add(im.id, im);
            }
        }

        public void APIRequestWithToken<K>(Action<K> callback, params Tuple<string, string>[] getParameters)
            where K : Response
        {
            APIRequest(callback, getParameters, new Tuple<string, string>[0], APIToken);
        }

        public void TestAuth(Action<AuthTestResponse> callback)
        {
            APIRequestWithToken(callback);
        }

        public void GetUserList(Action<UserListResponse> callback)
        {
            APIRequestWithToken(callback);
        }

        public void GetUserByEmail(Action<UserEmailLookupResponse> callback, string email)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("email", email));
        }

        public void ChannelsCreate(Action<ChannelCreateResponse> callback, string name)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("name", name));
        }

        public void ChannelsInvite(Action<ChannelInviteResponse> callback, string userId, string channelId)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("user", userId));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GetConversationsList(Action<ConversationsListResponse> callback, string cursor = "",
            bool ExcludeArchived = true, int limit = 100, string[] types = null)
        {
            var parameters = new List<Tuple<string, string>>
            {
                Tuple.Create("exclude_archived", ExcludeArchived ? "1" : "0")
            };
            if (limit > 0)
                parameters.Add(Tuple.Create("limit", limit.ToString()));
            if (types != null && types.Any())
                parameters.Add(Tuple.Create("types", string.Join(",", types)));
            if (!string.IsNullOrEmpty(cursor))
                parameters.Add(Tuple.Create("cursor", cursor));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GetChannelList(Action<ChannelListResponse> callback, bool ExcludeArchived = true)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("exclude_archived", ExcludeArchived ? "1" : "0"));
        }

        public void GetGroupsList(Action<GroupListResponse> callback, bool ExcludeArchived = true)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("exclude_archived", ExcludeArchived ? "1" : "0"));
        }

        public void GetDirectMessageList(Action<DirectMessageConversationListResponse> callback)
        {
            APIRequestWithToken(callback);
        }

        public void GetFiles(Action<FileListResponse> callback, string userId = null, DateTime? from = null,
            DateTime? to = null, int? count = null, int? page = null, FileTypes types = FileTypes.all,
            string channel = null)
        {
            var parameters = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(userId))
                parameters.Add(new Tuple<string, string>("user", userId));

            if (from.HasValue)
                parameters.Add(new Tuple<string, string>("ts_from", from.Value.ToProperTimeStamp()));

            if (to.HasValue)
                parameters.Add(new Tuple<string, string>("ts_to", to.Value.ToProperTimeStamp()));

            if (!types.HasFlag(FileTypes.all))
            {
                var values = (FileTypes[]) Enum.GetValues(typeof(FileTypes));

                var building = new StringBuilder();
                var first = true;
                for (var i = 0; i < values.Length; ++i)
                {
                    if (types.HasFlag(values[i]))
                    {
                        if (!first) building.Append(",");

                        building.Append(values[i].ToString());

                        first = false;
                    }
                }

                if (building.Length > 0)
                    parameters.Add(new Tuple<string, string>("types", building.ToString()));
            }

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            if (!string.IsNullOrEmpty(channel))
                parameters.Add(new Tuple<string, string>("channel", channel));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        private void GetHistory<K>(Action<K> historyCallback, string channel, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
            where K : MessageHistory
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("channel", channel));

            if (latest.HasValue)
                parameters.Add(new Tuple<string, string>("latest", latest.Value.ToProperTimeStamp()));
            if (oldest.HasValue)
                parameters.Add(new Tuple<string, string>("oldest", oldest.Value.ToProperTimeStamp()));
            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));
            if (unreads.HasValue)
                parameters.Add(new Tuple<string, string>("unreads", unreads.Value ? "1" : "0"));

            APIRequestWithToken(historyCallback, parameters.ToArray());
        }

        public void GetChannelHistory(Action<ChannelMessageHistory> callback, Channel channelInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, channelInfo.id, latest, oldest, count, unreads);
        }

        public void GetDirectMessageHistory(Action<MessageHistory> callback, DirectMessageConversation conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, conversationInfo.id, latest, oldest, count, unreads);
        }

        public void GetGroupHistory(Action<GroupMessageHistory> callback, Channel groupInfo, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, groupInfo.id, latest, oldest, count, unreads);
        }

        public void GetConversationsHistory(Action<ConversationsMessageHistory> callback, Channel conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, conversationInfo.id, latest, oldest, count, unreads);
        }

        public void MarkChannel(Action<MarkResponse> callback, string channelId, DateTime ts)
        {
            APIRequestWithToken(callback,
                new Tuple<string, string>("channel", channelId),
                new Tuple<string, string>("ts", ts.ToProperTimeStamp())
            );
        }

        public void GetFileInfo(Action<FileInfoResponse> callback, string fileId, int? page = null, int? count = null)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("file", fileId));

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }


        public void SearchAll(Action<SearchResponseAll> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("query", query));

            if (sorting != null)
                parameters.Add(new Tuple<string, string>("sort", sorting));

            if (direction.HasValue)
                parameters.Add(new Tuple<string, string>("sort_dir", direction.Value.ToString()));

            if (enableHighlights)
                parameters.Add(new Tuple<string, string>("highlight", "1"));

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void SearchMessages(Action<SearchResponseMessages> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("query", query));

            if (sorting != null)
                parameters.Add(new Tuple<string, string>("sort", sorting));

            if (direction.HasValue)
                parameters.Add(new Tuple<string, string>("sort_dir", direction.Value.ToString()));

            if (enableHighlights)
                parameters.Add(new Tuple<string, string>("highlight", "1"));

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void SearchFiles(Action<SearchResponseFiles> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters = new List<Tuple<string, string>>();
            parameters.Add(new Tuple<string, string>("query", query));

            if (sorting != null)
                parameters.Add(new Tuple<string, string>("sort", sorting));

            if (direction.HasValue)
                parameters.Add(new Tuple<string, string>("sort_dir", direction.Value.ToString()));

            if (enableHighlights)
                parameters.Add(new Tuple<string, string>("highlight", "1"));

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GetStars(Action<StarListResponse> callback, string userId = null, int? count = null,
            int? page = null)
        {
            var parameters = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(userId))
                parameters.Add(new Tuple<string, string>("user", userId));

            if (count.HasValue)
                parameters.Add(new Tuple<string, string>("count", count.Value.ToString()));

            if (page.HasValue)
                parameters.Add(new Tuple<string, string>("page", page.Value.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void DeleteMessage(Action<DeletedResponse> callback, string channelId, DateTime ts)
        {
            var parameters = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("ts", ts.ToProperTimeStamp()),
                new Tuple<string, string>("channel", channelId)
            };

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void EmitPresence(Action<PresenceResponse> callback, Presence status)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("presence", status.ToString()));
        }

        public void GetPreferences(Action<UserPreferencesResponse> callback)
        {
            APIRequestWithToken(callback);
        }

        public void EmitLogin(Action<LoginResponse> callback, string agent = "Inumedia.SlackAPI")
        {
            APIRequestWithToken(callback, new Tuple<string, string>("agent", agent));
        }

        public void Update(
            Action<UpdateResponse> callback,
            string ts,
            string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            IBlock[] blocks = null,
            Attachment[] attachments = null,
            bool as_user = false)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("ts", ts));
            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));

            if (!string.IsNullOrEmpty(botName))
                parameters.Add(new Tuple<string, string>("username", botName));

            if (!string.IsNullOrEmpty(parse))
                parameters.Add(new Tuple<string, string>("parse", parse));

            if (linkNames)
                parameters.Add(new Tuple<string, string>("link_names", "1"));

            if (blocks != null && blocks.Length > 0)
                parameters.Add(new Tuple<string, string>("blocks",
                    JsonConvert.SerializeObject(blocks, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));

            if (attachments != null && attachments.Length > 0)
                parameters.Add(new Tuple<string, string>("attachments",
                    JsonConvert.SerializeObject(attachments, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));


            parameters.Add(new Tuple<string, string>("as_user", as_user.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void JoinDirectMessageChannel(Action<JoinDirectMessageChannelResponse> callback, string user)
        {
            var param = new Tuple<string, string>("users", user);
            APIRequestWithToken(callback, param);
        }

        public void PostMessage(
            Action<PostMessageResponse> callback,
            string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            IBlock[] blocks = null,
            Attachment[] attachments = null,
            bool? unfurl_links = null,
            string icon_url = null,
            string icon_emoji = null,
            bool? as_user = null,
            string thread_ts = null)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));

            if (!string.IsNullOrEmpty(botName))
                parameters.Add(new Tuple<string, string>("username", botName));

            if (!string.IsNullOrEmpty(parse))
                parameters.Add(new Tuple<string, string>("parse", parse));

            if (linkNames)
                parameters.Add(new Tuple<string, string>("link_names", "1"));

            if (blocks != null && blocks.Length > 0)
                parameters.Add(new Tuple<string, string>("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Length > 0)
                parameters.Add(new Tuple<string, string>("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (unfurl_links.HasValue)
                parameters.Add(new Tuple<string, string>("unfurl_links", unfurl_links.Value ? "true" : "false"));

            if (!string.IsNullOrEmpty(icon_url))
                parameters.Add(new Tuple<string, string>("icon_url", icon_url));

            if (!string.IsNullOrEmpty(icon_emoji))
                parameters.Add(new Tuple<string, string>("icon_emoji", icon_emoji));

            if (as_user.HasValue)
                parameters.Add(new Tuple<string, string>("as_user", as_user.ToString()));

            if (!string.IsNullOrEmpty(thread_ts))
                parameters.Add(new Tuple<string, string>("thread_ts", thread_ts));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void PostEphemeralMessage(
            Action<PostEphemeralResponse> callback,
            string channelId,
            string text,
            string targetuser,
            string parse = null,
            bool linkNames = false,
            Block[] blocks = null,
            Attachment[] attachments = null,
            bool as_user = false,
            string thread_ts = null)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));
            parameters.Add(new Tuple<string, string>("user", targetuser));

            if (!string.IsNullOrEmpty(parse))
                parameters.Add(new Tuple<string, string>("parse", parse));

            if (linkNames)
                parameters.Add(new Tuple<string, string>("link_names", "1"));

            if (blocks != null && blocks.Length > 0)
                parameters.Add(new Tuple<string, string>("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Length > 0)
                parameters.Add(new Tuple<string, string>("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            parameters.Add(new Tuple<string, string>("as_user", as_user.ToString()));

            APIRequestWithToken(callback, parameters.ToArray());
        }


        public void ScheduleMessage(
            Action<ScheduleMessageResponse> callback,
            string channelId,
            string text,
            DateTime post_at,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            IBlock[] blocks = null,
            Attachment[] attachments = null,
            bool? unfurl_links = null,
            string icon_url = null,
            string icon_emoji = null,
            bool? as_user = null,
            string thread_ts = null)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("text", text));
            parameters.Add(new Tuple<string, string>("post_at",
                (post_at - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString()));

            if (!string.IsNullOrEmpty(botName))
                parameters.Add(new Tuple<string, string>("username", botName));

            if (!string.IsNullOrEmpty(parse))
                parameters.Add(new Tuple<string, string>("parse", parse));

            if (linkNames)
                parameters.Add(new Tuple<string, string>("link_names", "1"));

            if (blocks != null && blocks.Length > 0)
                parameters.Add(new Tuple<string, string>("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Length > 0)
                parameters.Add(new Tuple<string, string>("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (unfurl_links.HasValue)
                parameters.Add(new Tuple<string, string>("unfurl_links", unfurl_links.Value ? "true" : "false"));

            if (!string.IsNullOrEmpty(icon_url))
                parameters.Add(new Tuple<string, string>("icon_url", icon_url));

            if (!string.IsNullOrEmpty(icon_emoji))
                parameters.Add(new Tuple<string, string>("icon_emoji", icon_emoji));

            if (as_user.HasValue)
                parameters.Add(new Tuple<string, string>("as_user", as_user.ToString()));

            if (!string.IsNullOrEmpty(thread_ts))
                parameters.Add(new Tuple<string, string>("thread_ts", thread_ts));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void DialogOpen(
            Action<DialogOpenResponse> callback,
            string triggerId,
            Dialog dialog)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("trigger_id", triggerId));

            parameters.Add(new Tuple<string, string>("dialog",
                JsonConvert.SerializeObject(dialog,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void AddReaction(
            Action<ReactionAddedResponse> callback,
            string name = null,
            string channel = null,
            string timestamp = null)
        {
            var parameters = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(name))
                parameters.Add(new Tuple<string, string>("name", name));

            if (!string.IsNullOrEmpty(channel))
                parameters.Add(new Tuple<string, string>("channel", channel));

            if (!string.IsNullOrEmpty(timestamp))
                parameters.Add(new Tuple<string, string>("timestamp", timestamp));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void UploadFile(Action<FileUploadResponse> callback, byte[] fileData, string fileName,
            string[] channelIds, string title = null, string initialComment = null,
            string contentType = null)
        {
            var restRequest = CreateUploadRestRequest(fileData, fileName, channelIds, title, initialComment, contentType);
            var restResponse = RestClient.Execute<FileUploadResponse>(restRequest);
            callback(restResponse.Data);
        }

        public void DeleteFile(Action<FileDeleteResponse> callback, string file = null)
        {
            if (string.IsNullOrEmpty(file))
                return;

            APIRequestWithToken(callback, new Tuple<string, string>("file", file));
        }

        #region Groups

        public void GroupsArchive(Action<GroupArchiveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void GroupsClose(Action<GroupCloseResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void GroupsCreate(Action<GroupCreateResponse> callback, string name)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("name", name));
        }

        public void GroupsCreateChild(Action<GroupCreateChildResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void GroupsInvite(Action<GroupInviteResponse> callback, string userId, string channelId)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("user", userId));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GroupsKick(Action<GroupKickResponse> callback, string userId, string channelId)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("user", userId));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GroupsLeave(Action<GroupLeaveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void GroupsMark(Action<GroupMarkResponse> callback, string channelId, DateTime ts)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId),
                new Tuple<string, string>("ts", ts.ToProperTimeStamp()));
        }

        public void GroupsOpen(Action<GroupOpenResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void GroupsRename(Action<GroupRenameResponse> callback, string channelId, string name)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("name", name));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GroupsSetPurpose(Action<GroupSetPurposeResponse> callback, string channelId, string purpose)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("purpose", purpose));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GroupsSetTopic(Action<GroupSetPurposeResponse> callback, string channelId, string topic)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("topic", topic));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void GroupsUnarchive(Action<GroupUnarchiveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        #endregion

        #region Conversations

        public void ConversationsArchive(Action<ConversationsArchiveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void ConversationsClose(Action<ConversationsCloseResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void ConversationsCreate(Action<ConversationsCreateResponse> callback, string name)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("name", name));
        }

        public void ConversationsInvite(Action<ConversationsInviteResponse> callback, string channelId,
            string[] userIds)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("users", string.Join(",", userIds)));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsKick(Action<ConversationsKickResponse> callback, string channelId, string userId)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("user", userId));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsLeave(Action<ConversationsLeaveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void ConversationsMark(Action<ConversationsMarkResponse> callback, string channelId, DateTime ts)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("ts", ts.ToProperTimeStamp()));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsOpen(Action<ConversationsOpenResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        public void ConversationsRename(Action<ConversationsRenameResponse> callback, string channelId, string name)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("name", name));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsSetPurpose(Action<ConversationsSetPurposeResponse> callback, string channelId,
            string purpose)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("purpose", purpose));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsSetTopic(Action<ConversationsSetPurposeResponse> callback, string channelId,
            string topic)
        {
            var parameters = new List<Tuple<string, string>>();

            parameters.Add(new Tuple<string, string>("channel", channelId));
            parameters.Add(new Tuple<string, string>("topic", topic));

            APIRequestWithToken(callback, parameters.ToArray());
        }

        public void ConversationsUnarchive(Action<ConversationsUnarchiveResponse> callback, string channelId)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("channel", channelId));
        }

        #endregion

        #region Users

        public void GetCounts(Action<UserCountsResponse> callback)
        {
            APIRequestWithToken(callback);
        }

        public void GetPresence(Action<UserGetPresenceResponse> callback, string user)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("user", user));
        }

        public void GetInfo(Action<UserInfoResponse> callback, string user)
        {
            APIRequestWithToken(callback, new Tuple<string, string>("user", user));
        }

        #endregion
    }
}