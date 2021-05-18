using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("dialog.open")]
    public class DialogOpenResponse : Response
    {
        public class ResponseMetadata
        {
            public string[] messages { get; set; }
        }
    }
}