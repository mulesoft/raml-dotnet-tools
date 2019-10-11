using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class DocumentationItem
	{
		public string Title { get; set; }
		public string Content { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	}
}