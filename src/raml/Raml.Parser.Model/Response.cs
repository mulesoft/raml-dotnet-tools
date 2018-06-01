using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class Response
	{
		public string Code { get; set; }
		public string Description { get; set; }
        public IDictionary<string, Parameter> Headers { get; set; }
		public IDictionary<string, MimeType> Body { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	}
}