using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class SecurityScheme: BasicInfo
	{
		public SecuritySchemeDescriptor DescribedBy { get; set; }
		public SecuritySettings Settings { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	}
}