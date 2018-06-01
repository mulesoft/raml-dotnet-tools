using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class QueryString
    {
        public QueryString()
        {
            Examples = new Dictionary<string, string>();
            Type = new string[0];
        }

        public IDictionary<string, string> Examples { get; set; }
        public string DisplayName { get; set; }
        public string[] Type { get; set; }
    }
}