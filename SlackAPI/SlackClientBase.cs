using System;
using System.Collections.Generic;
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
        protected readonly string ApiToken;
        protected IWebProxy ProxySettings;

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
            this.ProxySettings = proxySettings;
            RestClient = new RestClient {Proxy = proxySettings};
            ApiToken = token;
        }

        public string APIBaseLocation { get; set; } = "https://slack.com/api/";

        public string OAuthBaseLocation { get; set; } = "https://slack.com/oauth/v2/";


        private static void AppendTokenHeader(string token, RestRequest restRequest)
        {
            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Authorization", "Bearer " + token);
            }
        }

        private RestRequest CreateRestRequest<K>(ICollection<(string, string)> getParameters,
            ICollection<(string, string)> postParameters, bool withToken = false)
        {
            var path = RequestPathAttribute.GetRequestPath<K>();

            var requestUri = GetSlackUri(Path.Combine(APIBaseLocation, path.Path), getParameters);
            var restRequest = new RestRequest(requestUri) {RequestFormat = DataFormat.None};

            AppendTokenHeader(withToken ? ApiToken : null, restRequest);

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

        protected RestRequest CreateUploadRestRequest(byte[] fileData, string fileName, string[] channelIds,
            string title = null, string initialComment = null, string contentType = null)
        {
            Action<RestRequest> addFileMethod = (z) =>
            {
                var provider = new FileExtensionContentTypeProvider();

                if (string.IsNullOrWhiteSpace(contentType) && !provider.TryGetContentType(fileName, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                z.AddFileBytes("file", fileData, fileName, contentType);
            };
            return CreateUploadRestRequest(fileName, channelIds, title, initialComment, contentType, addFileMethod);
        }
        protected RestRequest CreateUploadRestRequest(FileInfo fileInfo, string[] channelIds,
            string title = null, string initialComment = null, string contentType = null)
        {
            Action<RestRequest> addFileMethod = (z) =>
            {
                var provider = new FileExtensionContentTypeProvider();

                if (string.IsNullOrWhiteSpace(contentType) && !provider.TryGetContentType(fileInfo.Name, out contentType))
                {
                    contentType = "application/octet-stream";
                }
                z.AddFileBytes("file", System.IO.File.ReadAllBytes(fileInfo.FullName), fileInfo.Name, contentType);
            };
            return CreateUploadRestRequest(fileInfo.Name, channelIds, title, initialComment, contentType, addFileMethod);
        }
        private RestRequest CreateUploadRestRequest(string fileName, string[] channelIds, string title, string initialComment,
            string contentType, Action<RestRequest> addFileMethod)
        {
            var target = new Uri(Path.Combine(APIBaseLocation, "files.upload"));
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

            AppendTokenHeader(ApiToken, restRequest);

            restRequest.AddOrUpdateParameter("channels", string.Join(",", channelIds));


            addFileMethod(restRequest);
            restRequest.AlwaysMultipartFormData = true;
            return restRequest;
        }


        protected async Task<K> ExecuteApiRequestAsync<K>(ICollection<(string, string)> getParameters,
            ICollection<(string, string)> postParameters,
            bool withToken = false)
            where K : Response
        {
            var restRequest = CreateRestRequest<K>(getParameters, postParameters, withToken);
            var restResponse = await RestClient.ExecuteAsync<K>(restRequest);
            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }
            return restResponse.Data;
        }

        protected Task<K> ExecuteApiRequestWithTokenAsync<K>()
            where K : Response
        {
            return ExecuteApiRequestWithTokenAsync<K>(new ValueTuple<string, string>[] { });
        }

        protected Task<K> ExecuteApiRequestWithTokenAsync<K>(params ValueTuple<string, string>[] postParameters)
            where K : Response
        {
            return ExecuteApiRequestAsync<K>(new ValueTuple<string, string>[] { }, postParameters, true);
        }

        protected void ExecuteAsyncWithCallback<T>(Action<T> callbackAction, Func<Task<T>> func)
        {
            callbackAction(func().GetAwaiter().GetResult());
        }

        protected Uri GetSlackUri(string path, ICollection<(string, string)> getParameters)
        {
            string parameters = default;

            if (getParameters != null && getParameters.Any())
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

        public void RegisterConverter(JsonConverter converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            Extensions.Converters.Add(converter);
        }
    }
}