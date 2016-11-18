using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class MimeType
	{
		public string Type { get; set; }
		public string Schema { get; set; }
		public string Example { get; set; }
		public string Description { get; set; }
		public IDictionary<string, Parameter> FormParameters { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	    public RamlType InlineType { get; set; }
	}
}