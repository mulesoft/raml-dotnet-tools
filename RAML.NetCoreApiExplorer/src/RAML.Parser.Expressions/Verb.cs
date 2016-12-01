using System.Collections.Generic;
using System.Linq;

namespace Raml.Parser.Expressions
{
	public class Verb
	{
		private readonly IDictionary<string, object> dynamicRaml;
		private readonly VerbType type;
	    private readonly string defaultMediaType;
	    private readonly bool isOptional;

		public Verb(IDictionary<string, object> dynamicRaml, VerbType type, string defaultMediaType, bool isOptional = false)
		{
			this.dynamicRaml = dynamicRaml;
			this.type = type;
		    this.defaultMediaType = defaultMediaType;
		    this.isOptional = isOptional;
		}

		public VerbType Type { get { return type; } }

		public bool IsOptional { get { return isOptional; } }

		public string Description { get { return dynamicRaml.ContainsKey("description") ? (string)dynamicRaml["description"]: null; } }

		public IDictionary<string,Parameter> Headers { get; set; }

		public IEnumerable<Response> Responses { get; set; }

		public MimeType Body { get; set; }
	}
}