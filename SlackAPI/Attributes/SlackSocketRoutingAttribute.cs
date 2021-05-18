using System;

namespace SlackAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class SlackSocketRoutingAttribute : Attribute
    {
         public string Type {get;set;}
         public string SubType {get;set;}
        public SlackSocketRoutingAttribute(string type, string subtype = null)
        {
            this.Type = type;
            this.SubType = subtype;
        }
    }
}