using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class Method : BasicInfo
	{
		public Method()
		{
			Headers = new Dictionary<string, Parameter>();
			Responses = new List<Response>();
			QueryParameters = new Dictionary<string, Parameter>();
			Body = new Dictionary<string, MimeType>();
			BaseUriParameters = new Dictionary<string, Parameter>();
			SecuredBy = new List<string>();
			Protocols = new List<Protocol>();
			Is = new List<string>();
		}

		public string Verb { get; set; }

        public IDictionary<string, Parameter> Headers { get; set; }

		public IEnumerable<Response> Responses { get; set; }

		public IDictionary<string, Parameter> QueryParameters { get; set; }

		public IDictionary<string, MimeType> Body { get; set; }

		public IDictionary<string, Parameter> BaseUriParameters { get; set; }

		public IEnumerable<string> SecuredBy { get; set; }

		public IEnumerable<Protocol> Protocols { get; set; }

		public IEnumerable<string> Is { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	}
}