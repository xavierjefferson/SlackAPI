using System;

namespace SlackAPI.Models
{
    public abstract class Response
    {
        /// <summary>
        ///     Should always be checked before trying to process a response.
        /// </summary>
        public bool ok { get; set; }

        public ResponseMetaData response_metadata { get; set; }

        /// <summary>
        ///     if ok is false, then this is the reason-code
        /// </summary>
        public string error { get; set; }

        public string needed { get; set; }
        public string provided { get; set; }

        public void AssertOk()
        {
            if (!ok)
                throw new InvalidOperationException(string.Format("An error occurred: {0}", error));
        }
    }
}