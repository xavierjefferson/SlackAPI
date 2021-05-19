using System.Collections.Generic;
using SlackAPI.Attributes;

namespace SlackAPI.Models.RPCMessages
{
    [RequestPath("auth.start")]
    public class AuthStartResponse : Response
    {
        public string email { get; set; }
        public string domain { get; set; }
        public List<UserTeamCombo> users {get; set;} = new List<UserTeamCombo>();

        /// <summary>
        ///     Path to create a new team?
        /// </summary>
        public string create { get; set; }
    }
}