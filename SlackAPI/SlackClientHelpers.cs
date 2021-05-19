using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SlackAPI.Models;
using SlackAPI.Models.RPCMessages;

namespace SlackAPI
{
    public class SlackClientHelpers : SlackClientBase
    {
        public SlackClientHelpers() : base(null)
        {
        }

        public SlackClientHelpers(IWebProxy proxySettings)
            : base(null, proxySettings)
        {
        }

        public void AuthSignin(Action<AuthSigninResponse> callback, string userId, string teamId, string password)
        {
            ExecuteAsyncWithCallback(callback, () => AuthSigninAsync(userId, teamId, password));
        }

        public Task<AuthSigninResponse> AuthSigninAsync(string userId, string teamId, string password)
        {
            return ExecuteApiRequestAsync<AuthSigninResponse>(new[]
            {
                ("user", userId),
                ("team", teamId),
                ("password", password)
            }, new ValueTuple<string, string>[0]);
        }

        private string BuildScope(SlackScope scope)
        {
            var builder = new StringBuilder();
            if ((int) (scope & SlackScope.Identify) != 0)
                builder.Append("identify");
            if ((int) (scope & SlackScope.Read) != 0)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append("read");
            }

            if ((int) (scope & SlackScope.Post) != 0)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append("post");
            }

            if ((int) (scope & SlackScope.Client) != 0)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append("client");
            }

            if ((int) (scope & SlackScope.Admin) != 0)
            {
                if (builder.Length > 0)
                    builder.Append(",");
                builder.Append("admin");
            }

            return builder.ToString();
        }

        public void FindTeam(Action<FindTeamResponse> callback, string team)
        {
            ExecuteAsyncWithCallback(callback, () => FindTeamAsync(team));
        }

        public Task<FindTeamResponse> FindTeamAsync(string team)
        {
            //This seems to accept both 'team.slack.com' and just plain 'team'.
            //Going to go with the latter.
            var domainName = ("domain", team);
            return ExecuteApiRequestAsync<FindTeamResponse>(new[] {domainName}, new ValueTuple<string, string>[0]);
        }

        public void GetAccessToken(Action<AccessTokenResponse> callback, string clientId, string clientSecret,
            string redirectUri, string code)
        {
            ExecuteAsyncWithCallback(callback, () => GetAccessTokenAsync(clientId, clientSecret, redirectUri, code));
        }

        public Task<AccessTokenResponse> GetAccessTokenAsync(string clientId, string clientSecret, string redirectUri,
            string code)
        {
            return ExecuteApiRequestAsync<AccessTokenResponse>(new[]
            {
                ("client_id", clientId),
                ("client_secret", clientSecret), ("code", code),
                ("redirect_uri", redirectUri)
            }, new ValueTuple<string, string>[] { });
        }

        public Uri GetAuthorizeUri(string clientId, IEnumerable<string> scopes, IEnumerable<string> user_scopes,
            string redirectUri = null,
            string state = null, string team = null)
        {
            if (scopes == null && user_scopes == null)
            {
                throw new ArgumentException(
                    $"Either one or both of {nameof(scopes)} or {nameof(user_scopes)} argument must be passed");
            }

            var args = new StringTupleList
            {
                ("client_id", clientId)
            };
            if (!string.IsNullOrWhiteSpace(redirectUri))
            {
                args.Add("redirect_uri", redirectUri);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                args.Add("state", state);
            }

            if (!string.IsNullOrWhiteSpace(team))
            {
                args.Add("team", team);
            }

            if (scopes != null)
            {
                args.Add("scope", string.Join(",", scopes));
            }

            if (user_scopes != null)
            {
                args.Add("user_scope", string.Join(",", user_scopes));
            }

            return GetSlackUri(OAuthBaseLocation + "authorize", args.ToArray());
        }

        [Obsolete]
        public Uri GetAuthorizeUri(string clientId, SlackScope scopes, string redirectUri = null, string state = null,
            string team = null)
        {
            var theScopes = BuildScope(scopes);

            return GetSlackUri(OAuthBaseLocation + "authorize", new[]
            {
                ("client_id", clientId),
                ("redirect_uri", redirectUri),
                ("state", state),
                ("scope", theScopes),
                ("team", team)
            });
        }

        [Obsolete("Please use the OAuth method for authenticating users")]
        public void StartAuth(Action<AuthStartResponse> callback, string email)
        {
            ExecuteAsyncWithCallback(callback, () => StartAuthAsync(email));
        }

        [Obsolete("Please use the OAuth method for authenticating users")]
        public Task<AuthStartResponse> StartAuthAsync(string email)
        {
            return ExecuteApiRequestAsync<AuthStartResponse>(new[] {("email", email)},
                new ValueTuple<string, string>[0]);
        }
    }
}