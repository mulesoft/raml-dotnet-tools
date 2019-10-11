using System.Collections.Generic;
using System.Linq;

namespace Raml.Parser.Expressions
{
	public class BasicInfo
	{
		public string Description { get; set; }

		public IDictionary<string, IDictionary<string, string>> Type { get; set; }

        public string GetSingleType()
        {
            return Type != null && Type.Any() ? Type.First().Key : string.Empty;
        }
	}
}