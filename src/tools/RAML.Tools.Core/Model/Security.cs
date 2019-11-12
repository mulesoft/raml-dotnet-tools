using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class Security
    {
        public string AuthorizationUri { get; set; }
        public string AccessTokenUri { get; set; }

        public IEnumerable<string> AuthorizationGrants { get; set; }

        public IEnumerable<string> Scopes { get; set; }

        public string RequestTokenUri  { get; set; } // Oauth 1

        public string TokenCredentialsUri  { get; set; } // Oauth 1

        public IEnumerable<GeneratorParameter> Headers { get; set; }

        public int HeadersCount
        {
            get { return Headers == null ? 0 : Headers.Count(); }
        }

        public IEnumerable<GeneratorParameter> QueryParameters { get; set; }

        public int QueryParametersCount
        {
            get { return QueryParameters == null ? 0 : QueryParameters.Count(); }
        }
    }
}