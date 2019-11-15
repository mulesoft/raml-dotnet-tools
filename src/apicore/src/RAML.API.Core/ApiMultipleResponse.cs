using System.Linq;
using System.Net;
#if PORTABLE
using System.Reflection;
#endif

namespace RAML.Api.Core
{
	public class ApiMultipleResponse : ApiMultipleObject
	{
		public void SetPropertyByStatusCode(HttpStatusCode statusCode, object model)
		{
			if (!names.ContainsKey(statusCode))
				return;

			var propName = names[statusCode];
#if !PORTABLE
			GetType().GetProperties().First(p => p.Name == propName).SetValue(this, model);
#else
            GetType().GetTypeInfo().DeclaredProperties.First(p => p.Name == propName).SetValue(this, model);
#endif
		}
	}
}