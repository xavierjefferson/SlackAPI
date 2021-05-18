using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using RestSharp;
using SlackAPI.Attributes;
using SlackAPI.Models;

namespace SlackAPI
{
    public abstract class SlackClientBase
    {
        protected readonly string APIToken;
        protected readonly IWebProxy proxySettings;

        protected RestClient RestClient;

        static SlackClientBase()
        {
            // Force Tls 1.2 for Slack
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        protected SlackClientBase(string token) : this(token, null)
        {
        }

        protected SlackClientBase(string token, IWebProxy proxySettings)
        {
            RestClient = new RestClient {Proxy = proxySettings};
            APIToken = token;
        }

        public string OAuthBaseLocation { get; set; } = "https://slack.com/oauth/v2/";

        public string APIBaseLocation { get; set; } = "https://slack.com/api/";

        protected Uri GetSlackUri(string path, Tuple<string, string>[] getParameters)
        {
            string parameters = default;

            if (getParameters != null && getParameters.Length > 0)
            {
                parameters = getParameters
                    .Where(x => x.Item2 != null)
                    .Select(a =>
                    {
                        try
                        {
                            return string.Format("{0}={1}", Uri.EscapeDataString(a.Item1),
                                Uri.EscapeDataString(a.Item2));
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(string.Format("Failed when processing '{0}'.", a), ex);
                        }
                    })
                    .Aggregate((a, b) =>
                    {
                        if (string.IsNullOrEmpty(a))
                            return b;
                        return string.Format("{0}&{1}", a, b);
                    });
            }

            Uri requestUri = default;

            if (!string.IsNullOrEmpty(parameters))
                requestUri = new Uri(string.Format("{0}?{1}", path, parameters));
            else
                requestUri = new Uri(path);

            return requestUri;
        }

        private RestRequest GetRestRequest<K>(Tuple<string, string>[] getParameters,
            Tuple<string, string>[] postParameters, string token = "")
        {
            var path = RequestPathAttribute.GetRequestPath<K>();

            var requestUri = GetSlackUri(Path.Combine(APIBaseLocation, path.Path), getParameters);
            var restRequest = new RestRequest(requestUri) {RequestFormat = DataFormat.None};

            AppendTokenHeader(token, restRequest);

            restRequest.Method = postParameters.Any() ? Method.POST : Method.GET;
            if (postParameters.Any())
            {
                restRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
            }

            foreach (var tuple in postParameters)
            {
                restRequest.AddOrUpdateParameter(tuple.Item1, tuple.Item2);
            }

            return restRequest;
        }

        private static void AppendTokenHeader(string token, RestRequest restRequest)
        {
            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Authorization", "Bearer " + token);
            }
        }

        protected void APIRequest<K>(Action<K> callback, Tuple<string, string>[] getParameters,
            Tuple<string, string>[] postParameters, string token = "")
            where K : Response
        {
            var restRequest = GetRestRequest<K>(getParameters, postParameters, token);
            var restResponse = RestClient.Execute<K>(restRequest);
            callback(restResponse.Data);
        }

        public async Task<K> APIRequestAsync<K>(Tuple<string, string>[] getParameters,
            Tuple<string, string>[] postParameters,
            string token = "")
            where K : Response
        {
            var restRequest = GetRestRequest<K>(getParameters, postParameters, token);
            var restResponse = await RestClient.ExecuteAsync<K>(restRequest);
            return restResponse.Data;
        }

        protected void APIGetRequest<K>(Action<K> callback, params Tuple<string, string>[] getParameters)
            where K : Response
        {
            APIRequest(callback, getParameters, new Tuple<string, string>[0]);
        }

        public Task<K> APIGetRequestAsync<K>(params Tuple<string, string>[] getParameters)
            where K : Response
        {
            return APIRequestAsync<K>(getParameters, new Tuple<string, string>[0]);
        }


        public void RegisterConverter(JsonConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            Extensions.Converters.Add(converter);
        }

        protected RestRequest CreateUploadRestRequest(byte[] fileData, string fileName, string[] channelIds,
            string title = null, string initialComment = null, string contentType = null)
        {
            var target = new Uri(Path.Combine(APIBaseLocation,  "files.upload"));
            var restRequest = new RestRequest(target, Method.POST);


            //File/Content
            if (!string.IsNullOrEmpty(contentType))
            {
                restRequest.AddOrUpdateParameter("filetype", contentType);
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                restRequest.AddOrUpdateParameter("filename", fileName);
            }

            if (!string.IsNullOrEmpty(title))
            {
                restRequest.AddOrUpdateParameter("title", title);
            }


            if (!string.IsNullOrEmpty(initialComment))
            {
                restRequest.AddOrUpdateParameter("initial_comment", initialComment);
            }

            AppendTokenHeader(APIToken, restRequest);

            restRequest.AddOrUpdateParameter("channels", string.Join(",", channelIds));
            var provider = new FileExtensionContentTypeProvider();
        
            if (string.IsNullOrWhiteSpace(contentType) && !provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }

            restRequest.AddFileBytes("file", fileData, fileName, contentType);
            restRequest.AlwaysMultipartFormData = true;
            return restRequest;
        }
    }
}