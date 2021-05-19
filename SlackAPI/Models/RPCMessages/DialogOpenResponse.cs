using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("dialog.open")]
    public class DialogOpenResponse : Response
    {
        public class ResponseMetadata
        {
            public List<string> messages {get; set;} = new List<string>();
        }
    }
}