using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SlackAPI.Models
{
    public class StringTupleList : List<ValueTuple<string,string>>
    {
        public void Add(string name, string value)
        {
            this.Add(name, value);
        }
    }
}