using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        public SlackClient(string token) : base(token)
        {
        }

        public SlackClient(string token, IWebProxy proxySettings)
            : base(token, proxySettings)
        {
        }

        public List<Bot> Bots { get; set; } = new List<Bot>();
        public Dictionary<string, Channel> ChannelLookup { get; set; }

        public List<Channel> Channels { get; set; } = new List<Channel>();

        public Dictionary<string, Conversation> ConversationLookup { get; set; }
        public Dictionary<string, DirectMessageConversation> DirectMessageLookup { get; set; }
        public List<DirectMessageConversation> DirectMessages { get; set; } = new List<DirectMessageConversation>();
        public Dictionary<string, Channel> GroupLookup { get; set; }
        public List<Channel> Groups { get; set; } = new List<Channel>();
        public User MyData { get; set; }

        public Self MySelf { get; set; }
        public Team MyTeam { get; set; }

        public List<string> StarredChannels { get; set; } = new List<string>();

        public Dictionary<string, User> UserLookup { get; set; }

        public List<User> Users { get; set; }

        public void AddReaction(
            Action<ReactionAddedResponse> callback,
            string name = null,
            string channel = null,
            string timestamp = null)
        {
            ExecuteAsyncWithCallback(callback, () => AddReactionAsync(name, channel, timestamp));
        }

        public Task<ReactionAddedResponse> AddReactionAsync(
            string name = null,
            string channel = null,
            string timestamp = null)
        {
            var parameters = GetAddReactionParameters(name, channel, timestamp);


            return ExecuteApiRequestWithTokenAsync<ReactionAddedResponse>(parameters.ToArray());
        }

        public void ChannelsCreate(Action<ChannelCreateResponse> callback, string name)
        {
            ExecuteAsyncWithCallback(callback, () => ChannelsCreateAsync(name));
        }

        public Task<ChannelCreateResponse> ChannelsCreateAsync(string name)
        {
            return ExecuteApiRequestWithTokenAsync<ChannelCreateResponse>(("name", name));
        }

        public Task<ChannelSetTopicResponse> ChannelSetTopicAsync(string channelId, string newTopic)
        {
            return ExecuteApiRequestWithTokenAsync<ChannelSetTopicResponse>(
                ("channel", channelId),
                ("topic", newTopic));
        }

        public void ChannelsInvite(Action<ChannelInviteResponse> callback, string userId, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ChannelsInviteAsync(userId, channelId));
        }

        public Task<ChannelInviteResponse> ChannelsInviteAsync(string userId, string channelId)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("user", userId);

            return ExecuteApiRequestWithTokenAsync<ChannelInviteResponse>(parameters.ToArray());
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

        public virtual async Task<LoginResponse> ConnectAsync()
        {
            var loginDetails = await EmitLoginAsync().ConfigureAwait(false);
            if (loginDetails.ok)
                Connected(loginDetails);

            return loginDetails;
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
            StarredChannels =
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

        public void ConversationsArchive(Action<ConversationsArchiveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsArchiveAsync(channelId));
        }

        public Task<ConversationsArchiveResponse> ConversationsArchiveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<ConversationsArchiveResponse>(
                ("channel", channelId));
        }

        public void ConversationsClose(Action<ConversationsCloseResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsCloseAsync(channelId));
        }

        public Task<ConversationsCloseResponse> ConversationsCloseAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<ConversationsCloseResponse>(
                ("channel", channelId));
        }

        public void ConversationsCreate(Action<ConversationsCreateResponse> callback, string name)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsCreateAsync(name));
        }

        public Task<ConversationsCreateResponse> ConversationsCreateAsync(string name, bool? isPrivate = null,
            string teamId = null)
        {
            var parameters = new StringTupleList();
            parameters.Add("name", name);

            if (isPrivate.HasValue)
                parameters.Add("is_private", isPrivate.Value ? "true" : "false");

            if (!string.IsNullOrEmpty(teamId))
                parameters.Add("team_id", teamId);

            return ExecuteApiRequestWithTokenAsync<ConversationsCreateResponse>(parameters.ToArray());
        }

        public void ConversationsInvite(Action<ConversationsInviteResponse> callback, string channelId,
            string[] userIds)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsInviteAsync(channelId, userIds));
        }

        public Task<ConversationsInviteResponse> ConversationsInviteAsync(string channelId, string[] userIds)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("users", string.Join(",", userIds));

            return ExecuteApiRequestWithTokenAsync<ConversationsInviteResponse>(parameters.ToArray());
        }

        public void ConversationsKick(Action<ConversationsKickResponse> callback, string channelId, string userId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsKickAsync(channelId, userId));
        }

        public Task<ConversationsKickResponse> ConversationsKickAsync(string channelId, string userId)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("user", userId);

            return ExecuteApiRequestWithTokenAsync<ConversationsKickResponse>(parameters.ToArray());
        }

        public void ConversationsLeave(Action<ConversationsLeaveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsLeaveAsync(channelId));
        }

        public Task<ConversationsLeaveResponse> ConversationsLeaveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<ConversationsLeaveResponse>(
                ("channel", channelId));
        }

        public void ConversationsMark(Action<ConversationsMarkResponse> callback, string channelId, DateTime ts)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsMarkAsync(channelId, ts));
        }

        public Task<ConversationsMarkResponse> ConversationsMarkAsync(string channelId, DateTime ts)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("ts", ts.ToProperTimeStamp());

            return ExecuteApiRequestWithTokenAsync<ConversationsMarkResponse>(parameters.ToArray());
        }

        public void ConversationsOpen(Action<ConversationsOpenResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsOpenAsync(channelId));
        }

        public Task<ConversationsOpenResponse> ConversationsOpenAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<ConversationsOpenResponse>(("channel", channelId));
        }

        public void ConversationsRename(Action<ConversationsRenameResponse> callback, string channelId, string name)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsRenameAsync(channelId, name));
        }

        public Task<ConversationsRenameResponse> ConversationsRenameAsync(string channelId, string name)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("name", name);

            return ExecuteApiRequestWithTokenAsync<ConversationsRenameResponse>(parameters.ToArray());
        }

        public void ConversationsSetPurpose(Action<ConversationsSetPurposeResponse> callback, string channelId,
            string purpose)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsSetPurposeAsync(channelId, purpose));
        }

        public Task<ConversationsSetPurposeResponse> ConversationsSetPurposeAsync(string channelId, string purpose)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("purpose", purpose);

            return ExecuteApiRequestWithTokenAsync<ConversationsSetPurposeResponse>(parameters.ToArray());
        }

        public void ConversationsSetTopic(Action<ConversationsSetTopicResponse> callback, string channelId,
            string topic)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsSetTopicAsync(channelId, topic));
        }

        public Task<ConversationsSetTopicResponse> ConversationsSetTopicAsync(string channelId, string topic)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("topic", topic);

            return ExecuteApiRequestWithTokenAsync<ConversationsSetTopicResponse>(parameters.ToArray());
        }

        public void ConversationsUnarchive(Action<ConversationsUnarchiveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => ConversationsUnarchiveAsync(channelId));
        }

        public Task<ConversationsUnarchiveResponse> ConversationsUnarchiveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<ConversationsUnarchiveResponse>(
                ("channel", channelId));
        }

        public void DeleteFile(Action<FileDeleteResponse> callback, string file = null)
        {
            ExecuteAsyncWithCallback(callback, () => DeleteFileAsync(file = null));
        }

        public async Task<FileDeleteResponse> DeleteFileAsync(string file = null)
        {
            if (string.IsNullOrEmpty(file))
                return null;

            return await ExecuteApiRequestWithTokenAsync<FileDeleteResponse>(("file", file));
        }

        public void DeleteMessage(Action<DeletedResponse> callback, string channelId, DateTime ts)
        {
            ExecuteAsyncWithCallback(callback, () => DeleteMessageAsync(channelId, ts));
        }

        public Task<DeletedResponse> DeleteMessageAsync(string channelId, DateTime ts)
        {
            var parameters = new StringTupleList
            {
                ("ts", ts.ToProperTimeStamp()),
                ("channel", channelId)
            };

            return ExecuteApiRequestWithTokenAsync<DeletedResponse>(parameters.ToArray());
        }

        public void DialogOpen(
            Action<DialogOpenResponse> callback,
            string triggerId,
            Dialog dialog)
        {
            ExecuteAsyncWithCallback(callback, () => DialogOpenAsync(triggerId, dialog));
        }

        public Task<DialogOpenResponse> DialogOpenAsync(
            string triggerId,
            Dialog dialog)
        {
            var parameters = new StringTupleList();

            parameters.Add("trigger_id", triggerId);

            parameters.Add(("dialog",
                JsonConvert.SerializeObject(dialog,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));

            return ExecuteApiRequestWithTokenAsync<DialogOpenResponse>(parameters.ToArray());
        }

        public void EmitLogin(Action<LoginResponse> callback, string agent = "Inumedia.SlackAPI")
        {
            ExecuteAsyncWithCallback(callback, () => EmitLoginAsync(agent));
        }

        public Task<LoginResponse> EmitLoginAsync(string agent = "Inumedia.SlackAPI")
        {
            return ExecuteApiRequestWithTokenAsync<LoginResponse>(("agent", agent));
        }

        public void EmitPresence(Action<PresenceResponse> callback, Presence status)
        {
            ExecuteAsyncWithCallback(callback, () => EmitPresenceAsync(status));
        }

        public Task<PresenceResponse> EmitPresenceAsync(Presence status)
        {
            return ExecuteApiRequestWithTokenAsync<PresenceResponse>(("presence", status.ToString()));
        }

        protected static StringTupleList GetAddReactionParameters(string name, string channel, string timestamp)
        {
            var parameters = new StringTupleList();

            if (!string.IsNullOrEmpty(name))
                parameters.Add("name", name);

            if (!string.IsNullOrEmpty(channel))
                parameters.Add("channel", channel);

            if (!string.IsNullOrEmpty(timestamp))
                parameters.Add("timestamp", timestamp);
            return parameters;
        }

        public void GetChannelHistory(Action<ChannelMessageHistory> callback, Channel channelInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, channelInfo.id, latest, oldest, count, unreads);
        }

        public Task<ChannelMessageHistory> GetChannelHistoryAsync(Channel channelInfo, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            return GetHistoryAsync<ChannelMessageHistory>(channelInfo.id, latest, oldest, count, unreads);
        }

        public void GetChannelList(Action<ChannelListResponse> callback, bool ExcludeArchived = true)
        {
            ExecuteAsyncWithCallback(callback, () => GetChannelListAsync(ExcludeArchived));
        }

        public Task<ChannelListResponse> GetChannelListAsync(bool ExcludeArchived = true)
        {
            return ExecuteApiRequestWithTokenAsync<ChannelListResponse>(("exclude_archived",
                ExcludeArchived ? "1" : "0"));
        }

        public void GetConversationsHistory(Action<ConversationsMessageHistory> callback, Channel conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            GetHistory(callback, conversationInfo.id, latest, oldest, count, unreads);
        }

        public Task<ConversationsMessageHistory> GetConversationsHistoryAsync(Channel conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            return GetHistoryAsync<ConversationsMessageHistory>(conversationInfo.id, latest, oldest, count, unreads);
        }

        public void GetConversationsList(Action<ConversationsListResponse> callback, string cursor = "",
            bool excludeArchived = true, int limit = 100, string[] types = null)
        {
            ExecuteAsyncWithCallback(callback, () => GetConversationsListAsync(cursor, excludeArchived, limit, types));
        }

        public Task<ConversationsListResponse> GetConversationsListAsync(string cursor = "",
            bool excludeArchived = true, int limit = 100, string[] types = null)
        {
            var parameters = new StringTupleList
            {
                ("exclude_archived", excludeArchived ? "1" : "0")
            };
            if (limit > 0)
                parameters.Add("limit", limit.ToString());
            if (types != null && types.Any())
                parameters.Add("types", string.Join(",", types));
            if (!string.IsNullOrEmpty(cursor))
                parameters.Add("cursor", cursor);

            return ExecuteApiRequestWithTokenAsync<ConversationsListResponse>(parameters.ToArray());
        }

        public void GetCounts(Action<UserCountsResponse> callback)
        {
            ExecuteAsyncWithCallback(callback, GetCountsAsync);
        }

        public Task<UserCountsResponse> GetCountsAsync()
        {
            return ExecuteApiRequestWithTokenAsync<UserCountsResponse>();
        }

        public void GetDirectMessageHistory(Action<MessageHistory> callback, DirectMessageConversation conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            ExecuteAsyncWithCallback(callback,
                () => GetDirectMessageHistoryAsync(conversationInfo, latest, oldest, count, unreads));
        }

        public Task<MessageHistory> GetDirectMessageHistoryAsync(DirectMessageConversation conversationInfo,
            DateTime? latest = null, DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            return GetHistoryAsync<MessageHistory>(conversationInfo.id, latest, oldest, count, unreads);
        }

        public void GetDirectMessageList(Action<DirectMessageConversationListResponse> callback)
        {
            ExecuteAsyncWithCallback(callback, GetDirectMessageListAsync);
        }

        public Task<DirectMessageConversationListResponse> GetDirectMessageListAsync()
        {
            return ExecuteApiRequestWithTokenAsync<DirectMessageConversationListResponse>();
        }

        public void GetFileInfo(Action<FileInfoResponse> callback, string fileId, int? page = null, int? count = null)
        {
            ExecuteAsyncWithCallback(callback, () => GetFileInfoAsync(fileId, page, count));
        }

        public Task<FileInfoResponse> GetFileInfoAsync(string fileId, int? page = null, int? count = null)
        {
            var parameters = new StringTupleList();

            parameters.Add("file", fileId);

            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters.Add("page", page.Value.ToString());

            return ExecuteApiRequestWithTokenAsync<FileInfoResponse>(parameters.ToArray());
        }

        public void GetFiles(Action<FileListResponse> callback, string userId = null, DateTime? from = null,
            DateTime? to = null, int? count = null, int? page = null, FileTypes types = FileTypes.all,
            string channel = null)
        {
            ExecuteAsyncWithCallback(callback, () => GetFilesAsync(userId, from, to, count, page, types, channel));
        }

        public Task<FileListResponse> GetFilesAsync(string userId = null, DateTime? from = null, DateTime? to = null,
            int? count = null, int? page = null, FileTypes types = FileTypes.all,
            string channel = null)
        {
            var parameters = GetFilesParameters(userId, from, to, count, page, types, channel);


            return ExecuteApiRequestWithTokenAsync<FileListResponse>(parameters.ToArray());
        }

        protected virtual StringTupleList GetFilesParameters(string userId, DateTime? from, DateTime? to, int? count,
            int? page, FileTypes types, string channel)
        {
            var parameters = new StringTupleList();

            if (!string.IsNullOrEmpty(userId))
                parameters.Add("user", userId);

            if (from.HasValue)
                parameters.Add("ts_from", from.Value.ToProperTimeStamp());

            if (to.HasValue)
                parameters.Add("ts_to", to.Value.ToProperTimeStamp());

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
                    parameters.Add("types", building.ToString());
            }

            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters.Add("page", page.Value.ToString());

            if (!string.IsNullOrEmpty(channel))
                parameters.Add("channel", channel);
            return parameters;
        }

        public void GetGroupHistory(Action<GroupMessageHistory> callback, Channel groupInfo, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            ExecuteAsyncWithCallback(callback, () => GetGroupHistoryAsync(groupInfo, latest, oldest, count, unreads));
            ;
        }

        public Task<GroupMessageHistory> GetGroupHistoryAsync(Channel groupInfo, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
        {
            return GetHistoryAsync<GroupMessageHistory>(groupInfo.id, latest, oldest, count, unreads);
        }

        public void GetGroupsList(Action<GroupListResponse> callback, bool ExcludeArchived = true)
        {
            ExecuteAsyncWithCallback(callback, () => GetGroupsListAsync(ExcludeArchived));
        }

        public Task<GroupListResponse> GetGroupsListAsync(bool ExcludeArchived = true)
        {
            return ExecuteApiRequestWithTokenAsync<GroupListResponse>(("exclude_archived",
                ExcludeArchived ? "1" : "0"));
        }

        private void GetHistory<K>(Action<K> historyCallback, string channel, DateTime? latest = null,
            DateTime? oldest = null, int? count = null, bool? unreads = false)
            where K : MessageHistory
        {
            ExecuteAsyncWithCallback(historyCallback,
                () => GetHistoryAsync<K>(channel, latest, oldest, count, unreads));
        }

        private Task<K> GetHistoryAsync<K>(string channel, DateTime? latest = null, DateTime? oldest = null,
            int? count = null, bool? unreads = false)
            where K : MessageHistory
        {
            var parameters = GetHistoryParameters<K>(channel, latest, oldest, count, unreads);


            return ExecuteApiRequestWithTokenAsync<K>(parameters.ToArray());
        }

        protected virtual StringTupleList GetHistoryParameters<K>(string channel, DateTime? latest, DateTime? oldest,
            int? count, bool? unreads) where K : MessageHistory
        {
            var parameters = new StringTupleList();
            parameters.Add("channel", channel);

            if (latest.HasValue)
                parameters.Add("latest", latest.Value.ToProperTimeStamp());
            if (oldest.HasValue)
                parameters.Add("oldest", oldest.Value.ToProperTimeStamp());
            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());
            if (unreads.HasValue)
                parameters.Add("unreads", unreads.Value ? "1" : "0");
            return parameters;
        }

        public void GetInfo(Action<UserInfoResponse> callback, string user)
        {
            ExecuteAsyncWithCallback(callback, () => GetInfoAsync(user));
        }

        public async Task<UserInfoResponse> GetInfoAsync(string user)
        {
            return await ExecuteApiRequestWithTokenAsync<UserInfoResponse>(("user", user));
        }

        public void GetPreferences(Action<UserPreferencesResponse> callback)
        {
            ExecuteAsyncWithCallback(callback, GetPreferencesAsync);
        }

        public Task<UserPreferencesResponse> GetPreferencesAsync()
        {
            return ExecuteApiRequestWithTokenAsync<UserPreferencesResponse>();
        }

        public void GetPresence(Action<UserGetPresenceResponse> callback, string user)
        {
            ExecuteAsyncWithCallback(callback, () => GetPresenceAsync(user));
        }

        public async Task<UserGetPresenceResponse> GetPresenceAsync(string user)
        {
            return await ExecuteApiRequestWithTokenAsync<UserGetPresenceResponse>(("user", user));
        }

        protected static StringTupleList GetSearchAllParameters(string query, string sorting,
            SearchSortDirection? direction,
            bool enableHighlights, int? count, int? page)
        {
            var parameters = new StringTupleList {{"query", query}};

            if (sorting != null)
                parameters.Add("sort", sorting);

            if (direction.HasValue)
                parameters.Add("sort_dir", direction.Value.ToString());

            if (enableHighlights)
                parameters.Add("highlight", "1");

            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters.Add("page", page.Value.ToString());
            return parameters;
        }

        protected static StringTupleList GetSearchMessagesParameters(string query, string sorting,
            SearchSortDirection? direction,
            bool enableHighlights, int? count, int? page)
        {
            var parameters = new StringTupleList();
            parameters.Add("query", query);

            if (sorting != null)
                parameters.Add("sort", sorting);

            if (direction.HasValue)
                parameters.Add("sort_dir", direction.Value.ToString());

            if (enableHighlights)
                parameters.Add("highlight", "1");

            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters.Add("page", page.Value.ToString());
            return parameters;
        }

        public void GetStars(Action<StarListResponse> callback, string userId = null, int? count = null,
            int? page = null)
        {
            ExecuteAsyncWithCallback(callback, () => GetStarsAsync(userId, count, page));
        }

        public Task<StarListResponse> GetStarsAsync(string userId = null, int? count = null, int? page = null)
        {
            var parameters = new StringTupleList();

            if (!string.IsNullOrEmpty(userId))
                parameters.Add("user", userId);

            if (count.HasValue)
                parameters.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters.Add("page", page.Value.ToString());

            return ExecuteApiRequestWithTokenAsync<StarListResponse>(parameters.ToArray());
        }

        public void GetUserByEmail(Action<UserEmailLookupResponse> callback, string email)
        {
            ExecuteAsyncWithCallback(callback, () => GetUserByEmailAsync(email));
        }

        public Task<UserEmailLookupResponse> GetUserByEmailAsync(string email)
        {
            return ExecuteApiRequestWithTokenAsync<UserEmailLookupResponse>(("email", email));
        }

        public void GetUserList(Action<UserListResponse> callback)
        {
            ExecuteAsyncWithCallback(callback, GetUserListAsync);
        }

        public Task<UserListResponse> GetUserListAsync()
        {
            return ExecuteApiRequestWithTokenAsync<UserListResponse>();
        }

        public void GroupsArchive(Action<GroupArchiveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsArchiveAsync(channelId));
        }

        public Task<GroupArchiveResponse> GroupsArchiveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupArchiveResponse>(("channel", channelId));
        }

        public void GroupsClose(Action<GroupCloseResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsCloseAsync(channelId));
        }

        public Task<GroupCloseResponse> GroupsCloseAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupCloseResponse>(("channel", channelId));
        }

        public void GroupsCreate(Action<GroupCreateResponse> callback, string name)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsCreateAsync(name));
        }

        public Task<GroupCreateResponse> GroupsCreateAsync(string name)
        {
            return ExecuteApiRequestWithTokenAsync<GroupCreateResponse>(("name", name));
        }

        public void GroupsCreateChild(Action<GroupCreateChildResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsCreateChildAsync(channelId));
        }

        public Task<GroupCreateChildResponse> GroupsCreateChildAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupCreateChildResponse>(("channel", channelId));
        }

        public void GroupsInvite(Action<GroupInviteResponse> callback, string userId, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsInviteAsync(userId, channelId));
        }

        public Task<GroupInviteResponse> GroupsInviteAsync(string userId, string channelId)
        {
            var parameters = new StringTupleList
            {
                ("channel", channelId), ("user", userId)
            };


            return ExecuteApiRequestWithTokenAsync<GroupInviteResponse>(parameters.ToArray());
        }

        public void GroupsKick(Action<GroupKickResponse> callback, string userId, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsKickAsync(userId, channelId));
        }

        public Task<GroupKickResponse> GroupsKickAsync(string userId, string channelId)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("user", userId);

            return ExecuteApiRequestWithTokenAsync<GroupKickResponse>(parameters.ToArray());
        }

        public void GroupsLeave(Action<GroupLeaveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsLeaveAsync(channelId));
        }

        public Task<GroupLeaveResponse> GroupsLeaveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupLeaveResponse>(("channel", channelId));
        }

        public void GroupsMark(Action<GroupMarkResponse> callback, string channelId, DateTime ts)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsMarkAsync(channelId, ts));
        }

        public Task<GroupMarkResponse> GroupsMarkAsync(string channelId, DateTime ts)
        {
            return ExecuteApiRequestWithTokenAsync<GroupMarkResponse>(("channel", channelId),
                ("ts", ts.ToProperTimeStamp()));
        }

        public void GroupsOpen(Action<GroupOpenResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsOpenAsync(channelId));
        }

        public Task<GroupOpenResponse> GroupsOpenAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupOpenResponse>(("channel", channelId));
        }

        public void GroupsRename(Action<GroupRenameResponse> callback, string channelId, string name)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsRenameAsync(channelId, name));
        }

        public Task<GroupRenameResponse> GroupsRenameAsync(string channelId, string name)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("name", name);

            return ExecuteApiRequestWithTokenAsync<GroupRenameResponse>(parameters.ToArray());
        }

        public void GroupsSetPurpose(Action<GroupSetPurposeResponse> callback, string channelId, string purpose)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsSetPurposeAsync(channelId, purpose));
        }

        public Task<GroupSetPurposeResponse> GroupsSetPurposeAsync(string channelId, string purpose)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("purpose", purpose);

            return ExecuteApiRequestWithTokenAsync<GroupSetPurposeResponse>(parameters.ToArray());
        }

        public void GroupsSetTopic(Action<GroupSetTopicResponse> callback, string channelId, string topic)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsSetTopicAsync(channelId, topic));
        }

        public Task<GroupSetTopicResponse> GroupsSetTopicAsync(string channelId, string topic)
        {
            var parameters = new StringTupleList();

            parameters.Add("channel", channelId);
            parameters.Add("topic", topic);

            return ExecuteApiRequestWithTokenAsync<GroupSetTopicResponse>(parameters.ToArray());
        }

        public void GroupsUnarchive(Action<GroupUnarchiveResponse> callback, string channelId)
        {
            ExecuteAsyncWithCallback(callback, () => GroupsUnarchiveAsync(channelId));
        }

        public Task<GroupUnarchiveResponse> GroupsUnarchiveAsync(string channelId)
        {
            return ExecuteApiRequestWithTokenAsync<GroupUnarchiveResponse>(("channel", channelId));
        }

        public void JoinDirectMessageChannel(Action<JoinDirectMessageChannelResponse> callback, string user)
        {
            ExecuteAsyncWithCallback(callback,
                () => ExecuteApiRequestWithTokenAsync<JoinDirectMessageChannelResponse>(("users", user)));
        }

        public Task<JoinDirectMessageChannelResponse> JoinDirectMessageChannelAsync(string user)
        {
            var param = ("user", user);
            return ExecuteApiRequestWithTokenAsync<JoinDirectMessageChannelResponse>(param);
        }

        public void MarkChannel(Action<MarkResponse> callback, string channelId, DateTime ts)
        {
            ExecuteAsyncWithCallback(callback, () => MarkChannelAsync(channelId, ts));
        }

        public Task<MarkResponse> MarkChannelAsync(string channelId, DateTime ts)
        {
            return ExecuteApiRequestWithTokenAsync<MarkResponse>(("channel", channelId),
                ("ts", ts.ToProperTimeStamp())
            );
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
            ExecuteAsyncWithCallback(callback, () => PostEphemeralMessageAsync(channelId, text, targetuser, parse,
                linkNames,
                blocks,
                attachments, as_user, thread_ts));
        }

        public Task<PostEphemeralResponse> PostEphemeralMessageAsync(
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
            var parameters1 = new StringTupleList();

            parameters1.Add("channel", channelId);
            parameters1.Add("text", text);
            parameters1.Add("user", targetuser);

            if (!string.IsNullOrEmpty(parse))
                parameters1.Add("parse", parse);

            if (linkNames)
                parameters1.Add("link_names", "1");

            if (blocks != null && blocks.Length > 0)
                parameters1.Add(("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Length > 0)
                parameters1.Add(("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            parameters1.Add("as_user", as_user.ToString());
            var parameters = parameters1;

            return ExecuteApiRequestWithTokenAsync<PostEphemeralResponse>(parameters.ToArray());
        }

        public void PostMessage(
            Action<PostMessageResponse> callback,
            string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            List<IBlock>  blocks = null,
            List<Attachment> attachments = null,
            bool? unfurl_links = null,
            string icon_url = null,
            string icon_emoji = null,
            bool? as_user = null,
            string thread_ts = null)
        {
            ExecuteAsyncWithCallback(callback, () => PostMessageAsync(channelId, text, botName, parse, linkNames,
                blocks,
                attachments,
                unfurl_links, icon_url, icon_emoji, as_user, thread_ts));
        }

        public Task<PostMessageResponse> PostMessageAsync(
            string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false,
            ICollection<IBlock>  blocks = null,
            ICollection<Attachment> attachments = null,
            bool? unfurl_links = null,
            string icon_url = null,
            string icon_emoji = null,
            bool? as_user = null,
            string thread_ts = null)
        {
            var parameters1 = new StringTupleList();

            parameters1.Add("channel", channelId);
            parameters1.Add("text", text);

            if (!string.IsNullOrEmpty(botName))
                parameters1.Add("username", botName);

            if (!string.IsNullOrEmpty(parse))
                parameters1.Add("parse", parse);

            if (linkNames)
                parameters1.Add("link_names", "1");

            if (blocks != null && blocks.Count > 0)
                parameters1.Add(("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Count > 0)
                parameters1.Add(("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (unfurl_links.HasValue)
                parameters1.Add("unfurl_links", unfurl_links.Value ? "true" : "false");

            if (!string.IsNullOrEmpty(icon_url))
                parameters1.Add("icon_url", icon_url);

            if (!string.IsNullOrEmpty(icon_emoji))
                parameters1.Add("icon_emoji", icon_emoji);

            if (as_user.HasValue)
                parameters1.Add("as_user", as_user.ToString());

            if (!string.IsNullOrEmpty(thread_ts))
                parameters1.Add("thread_ts", thread_ts);
            var parameters = parameters1;


            return ExecuteApiRequestWithTokenAsync<PostMessageResponse>(parameters.ToArray());
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
            ExecuteAsyncWithCallback(callback, () => ScheduleMessageAsync(channelId, text, post_at, botName, parse,
                linkNames, blocks,
                attachments, unfurl_links, icon_url, icon_emoji, as_user, thread_ts));
        }

        public Task<ScheduleMessageResponse> ScheduleMessageAsync(
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
            var parameters1 = new StringTupleList();

            parameters1.Add("channel", channelId);
            parameters1.Add("text", text);
            parameters1.Add(("post_at",
                (post_at - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString()));

            if (!string.IsNullOrEmpty(botName))
                parameters1.Add("username", botName);

            if (!string.IsNullOrEmpty(parse))
                parameters1.Add("parse", parse);

            if (linkNames)
                parameters1.Add("link_names", "1");

            if (blocks != null && blocks.Length > 0)
                parameters1.Add(("blocks",
                    JsonConvert.SerializeObject(blocks, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (attachments != null && attachments.Length > 0)
                parameters1.Add(("attachments",
                    JsonConvert.SerializeObject(attachments, Formatting.None,
                        new JsonSerializerSettings // Shouldn't include a not set property
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        })));

            if (unfurl_links.HasValue)
                parameters1.Add("unfurl_links", unfurl_links.Value ? "true" : "false");

            if (!string.IsNullOrEmpty(icon_url))
                parameters1.Add("icon_url", icon_url);

            if (!string.IsNullOrEmpty(icon_emoji))
                parameters1.Add("icon_emoji", icon_emoji);

            if (as_user.HasValue)
                parameters1.Add("as_user", as_user.ToString());

            if (!string.IsNullOrEmpty(thread_ts))
                parameters1.Add("thread_ts", thread_ts);
            var parameters = parameters1;


            return ExecuteApiRequestWithTokenAsync<ScheduleMessageResponse>(parameters.ToArray());
        }

        public void SearchAll(Action<SearchResponseAll> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            ExecuteAsyncWithCallback(callback,
                () => SearchAllAsync(query, sorting, direction, enableHighlights, count, page));
        }

        public Task<SearchResponseAll> SearchAllAsync(string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters = GetSearchAllParameters(query, sorting, direction, enableHighlights, count, page);


            return ExecuteApiRequestWithTokenAsync<SearchResponseAll>(parameters.ToArray());
        }

        public void SearchFiles(Action<SearchResponseFiles> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            ExecuteAsyncWithCallback(callback,
                () => SearchFilesAsync(query, sorting, direction, enableHighlights, count, page));
        }

        public Task<SearchResponseFiles> SearchFilesAsync(string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters1 = new StringTupleList {{"query", query}};

            if (sorting != null)
                parameters1.Add("sort", sorting);

            if (direction.HasValue)
                parameters1.Add("sort_dir", direction.Value.ToString());

            if (enableHighlights)
                parameters1.Add("highlight", "1");

            if (count.HasValue)
                parameters1.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters1.Add("page", page.Value.ToString());
            var parameters = parameters1;


            return ExecuteApiRequestWithTokenAsync<SearchResponseFiles>(parameters.ToArray());
        }

        public void SearchMessages(Action<SearchResponseMessages> callback, string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            ExecuteAsyncWithCallback(callback,
                () => SearchMessagesAsync(query, sorting, direction, enableHighlights, count, page));
        }

        public Task<SearchResponseMessages> SearchMessagesAsync(string query, string sorting = null,
            SearchSortDirection? direction = null, bool enableHighlights = false, int? count = null, int? page = null)
        {
            var parameters1 = new StringTupleList();
            parameters1.Add("query", query);

            if (sorting != null)
                parameters1.Add("sort", sorting);

            if (direction.HasValue)
                parameters1.Add("sort_dir", direction.Value.ToString());

            if (enableHighlights)
                parameters1.Add("highlight", "1");

            if (count.HasValue)
                parameters1.Add("count", count.Value.ToString());

            if (page.HasValue)
                parameters1.Add("page", page.Value.ToString());
            var parameters = parameters1;


            return ExecuteApiRequestWithTokenAsync<SearchResponseMessages>(parameters.ToArray());
        }

        public void TestAuth(Action<AuthTestResponse> callback)
        {
            ExecuteAsyncWithCallback(callback, TestAuthAsync);
        }

        public Task<AuthTestResponse> TestAuthAsync()
        {
            return ExecuteApiRequestWithTokenAsync<AuthTestResponse>();
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
            ExecuteAsyncWithCallback(callback, () => UpdateAsync(ts, channelId, text, botName, parse, linkNames, blocks,
                attachments,
                as_user));
        }

        public Task<UpdateResponse> UpdateAsync(string ts,
            string channelId,
            string text,
            string botName = null,
            string parse = null,
            bool linkNames = false, IBlock[] blocks = null,
            Attachment[] attachments = null,
            bool as_user = false)
        {
            var parameters1 = new StringTupleList();

            parameters1.Add("ts", ts);
            parameters1.Add("channel", channelId);
            parameters1.Add("text", text);

            if (!string.IsNullOrEmpty(botName))
                parameters1.Add("username", botName);

            if (!string.IsNullOrEmpty(parse))
                parameters1.Add("parse", parse);

            if (linkNames)
                parameters1.Add("link_names", "1");

            if (blocks != null && blocks.Length > 0)
                parameters1.Add(("blocks",
                    JsonConvert.SerializeObject(blocks, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));

            if (attachments != null && attachments.Length > 0)
                parameters1.Add(("attachments",
                    JsonConvert.SerializeObject(attachments, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    })));


            parameters1.Add("as_user", as_user.ToString());
            var parameters = parameters1;


            return ExecuteApiRequestWithTokenAsync<UpdateResponse>(parameters.ToArray());
        }

        public void UploadFile(Action<FileUploadResponse> callback, byte[] fileData, string fileName,
            string[] channelIds, string title = null, string initialComment = null,
            string contentType = null)
        {
            ExecuteAsyncWithCallback(callback,
                () => UploadFileAsync(fileData, fileName, channelIds, title, initialComment, contentType));
        }

        public void UploadFile(Action<FileUploadResponse> callback, FileInfo fileInfo,
            string[] channelIds, string title = null, string initialComment = null,
            string contentType = null)
        {
            ExecuteAsyncWithCallback(callback,
                () => UploadFileAsync(fileInfo, channelIds, title, initialComment, contentType));
        }

        public async Task<FileUploadResponse> UploadFileAsync(byte[] fileData, string fileName, string[] channelIds,
            string title = null, string initialComment = null, string contentType = null)
        {
            var restRequest = CreateUploadRestRequest(fileData, fileName, channelIds, title, initialComment,
                contentType);
            var restResponse = await RestClient.ExecuteAsync<FileUploadResponse>(restRequest);
            return restResponse.Data;
        }

        public async Task<FileUploadResponse> UploadFileAsync(FileInfo fileInfo, string[] channelIds,
            string title = null, string initialComment = null, string contentType = null)
        {
            var restRequest = CreateUploadRestRequest(fileInfo, channelIds, title, initialComment,
                contentType);
            var restResponse = await RestClient.ExecuteAsync<FileUploadResponse>(restRequest);
            return restResponse.Data;
        }
    }
}