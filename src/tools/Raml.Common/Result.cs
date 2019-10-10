using System.Collections.Generic;

namespace Raml.Common
{
    public class Result
    {
        public bool HasErrors { get; set; }
        public string Content { get; set; }
        public string Errors { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }
}