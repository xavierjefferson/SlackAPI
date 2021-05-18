using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("auth.start")]
    public class AuthStartResponse : Response
    {
        public string email { get; set; }
        public string domain { get; set; }
        public UserTeamCombo[] users { get; set; }

        /// <summary>
        ///     Path to create a new team?
        /// </summary>
        public string create { get; set; }
    }
}