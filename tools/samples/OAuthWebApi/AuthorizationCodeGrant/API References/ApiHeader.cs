using System.Collections.Generic;
using System.Linq;

namespace Raml.Client.Common
{
	public class ApiHeader
	{
		public IDictionary<string, string> Headers
		{
			get
			{
				var properties = this.GetType().GetProperties().Where(p => p.Name != "Headers" && p.GetValue(this) != null);
				return properties.ToDictionary(prop => prop.Name, prop => prop.GetValue(this).ToString());
			}
		}
	}
}