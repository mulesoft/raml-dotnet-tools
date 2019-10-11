using System.Linq;
using System.Net;

namespace Raml.Client.Common
{
	public class ApiMultipleResponse : ApiMultipleObject
	{
		public void SetPropertyByStatusCode(HttpStatusCode statusCode, object model)
		{
			if (!names.ContainsKey(statusCode))
				return;

			var propName = names[statusCode];
			GetType().GetProperties().First(p => p.Name == propName).SetValue(this, model);
		}
	}
}