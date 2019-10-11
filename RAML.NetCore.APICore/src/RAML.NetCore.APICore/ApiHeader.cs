using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RAML.Api.Core
{
	public class ApiHeader
	{
		public IDictionary<string, string> Headers
		{
			get
			{

                var properties = this.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.Name != "Headers" && p.GetValue(this) != null);
				return properties.ToDictionary(prop => prop.Name, prop => prop.GetValue(this).ToString());
			}
		}
	}
}