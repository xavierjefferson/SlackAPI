using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace SlackAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RequestPathAttribute : Attribute
    {
        private static readonly ConcurrentDictionary<Type, RequestPathAttribute> paths =
            new ConcurrentDictionary<Type, RequestPathAttribute>();

        //See notes in Slack:APIRequest<K>
        public string Path { get; }
        public bool UsePrimaryApi { get; }

        public RequestPathAttribute(string requestPath, bool isPrimaryAPI = true)
        {
            Path = requestPath;
            UsePrimaryApi = isPrimaryAPI;
        }

        public static RequestPathAttribute GetRequestPath<K>()
        {
            var t = typeof(K);
            if (paths.TryGetValue(t, out var path))
                return path;

            var info = t.GetTypeInfo();

            path = info.GetCustomAttribute<RequestPathAttribute>();
            if (path == null)
                throw new InvalidOperationException(string.Format("No valid request path for {0}", t.Name));

            try
            {
                // Some other thread may have already placed the path to the dictionary, 
                // the original one will remain there and current one will be GCed once it
                // gets out of scope of this request.
                paths.TryAdd(t, path);
            }
            catch (Exception e)
            {
                // There is a slight chance of TryAdd throwing, we want to be extra safe
                // so we just consume it and leave the dictionary as-is, next call of
                // the same function will try to add the path again.
                // This may be removed in the future if TryAdd is verified as safe.
                // See #190.
                Trace.TraceError(e.ToString());
            }

            return path;
        }
    }
}