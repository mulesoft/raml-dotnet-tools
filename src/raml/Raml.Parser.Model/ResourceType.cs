using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class ResourceType
	{
	    public string Type { get; set; }
	    public IEnumerable<string> Is { get; set; }
		public Verb Get { get; set; }

		public Verb Post { get; set; }

		public Verb Put { get; set; }

		public Verb Delete { get; set; }

		public Verb Patch { get; set; }

		public Verb Options { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }

        public IDictionary<string, Parameter> UriParameters { get; set; }

	}
}