using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class SecuritySchemeDescriptor
	{
		public IDictionary<string, Parameter> Headers { get; set; }

		public IDictionary<string, Parameter> QueryParameters { get; set; }

		public IDictionary<string, Response> Responses { get; set; }

        public IDictionary<string, Parameter> QueryString { get; set; } // TODO: check

        public string DisplayName { get; set; }

        public string Description { get; set; }

	}
}