using System.Collections.Generic;
using System.Net;

namespace AMF.Common
{
    public class RamlIncludesManagerResult
    {
        public RamlIncludesManagerResult(string modifiedContents, IEnumerable<string> includedFiles)
        {
            ModifiedContents = modifiedContents;
            IncludedFiles = includedFiles;
            IsSuccess = true;
        }

        public RamlIncludesManagerResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            IncludedFiles = new List<string>();
        }

        public string ModifiedContents { get; private set; }
        public IEnumerable<string> IncludedFiles { get; private set; }
        public bool IsSuccess { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
    }
}