using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class SecuritySettings
	{
		public string RequestTokenUri { get; set; } // Oauth 1

		public string TokenCredentialsUri { get; set; } // Oauth 1

		public string AuthorizationUri { get; set; } // Oauth 1 and 2

		public string AccessTokenUri { get; set; } // Oauth 2

		public IEnumerable<string> AuthorizationGrants  { get; set; } // Oauth 2

		public IEnumerable<string> Scopes { get; set; } // Oauth 2
	}
}