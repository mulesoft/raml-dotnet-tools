using System.Linq;
using System.Net.Http.Headers;
using Raml.Common;

namespace Raml.Client.Common
{
	public class ApiResponseHeader
	{
		public void SetProperties(HttpResponseHeaders headers)
		{
			var properties = this.GetType().GetProperties().Where(p => p.GetValue(this) != null);
			foreach (var prop in properties.Where(prop => headers.Any(h => h.Key == prop.Name)))
			{
				prop.SetValue(this, headers.First(h => NetNamingMapper.GetPropertyName(h.Key) == prop.Name));
			}
		}
	}
}