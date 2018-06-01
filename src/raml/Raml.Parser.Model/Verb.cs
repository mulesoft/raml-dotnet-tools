using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class Verb
	{
	    public VerbType Type { get; set; }

		public bool IsOptional { get; set; }

		public string Description { get; set; }

	    public IDictionary<string,Parameter> Headers { get; set; }

	    public IEnumerable<Response> Responses { get; set; }

	    public MimeType Body { get; set; }

        public IDictionary<string, Parameter> QueryParameters { get; set; }
	}
}