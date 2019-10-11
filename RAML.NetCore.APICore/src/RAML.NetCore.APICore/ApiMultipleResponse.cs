using System.Linq;
using System.Net;
using System.Reflection;

namespace RAML.Api.Core
{
	public class ApiMultipleResponse : ApiMultipleObject
	{
		public void SetPropertyByStatusCode(HttpStatusCode statusCode, object model)
		{
			if (!names.ContainsKey(statusCode))
				return;

			var propName = names[statusCode];
            GetType().GetTypeInfo().DeclaredProperties.First(p => p.Name == propName).SetValue(this, model);

		}
	}
}