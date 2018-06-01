using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
	public class Parameter
	{
        public Parameter()
        {
            Type = "string";
        }

		public string Type { get; set; }
		public bool Required { get; set; }
		public string DisplayName { get; set; }
        public string LibraryName { get; set; }
		public string Description { get; set; }
		public IEnumerable<string> Enum { get; set; }
		public bool Repeat { get; set; }
		public string Example { get; set; }
		public string Default { get; set; }
		public string Pattern { get; set; }
		public int? MinLength { get; set; }
		public int? MaxLength { get; set; }
		public decimal? Minimum { get; set; }
		public decimal? Maximum { get; set; }
	    public IDictionary<string, object> Annotations { get; set; }
	    public string Format { get; set; }
	}
}