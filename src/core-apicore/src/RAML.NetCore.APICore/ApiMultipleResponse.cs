using System.Linq;
using System.Net;
using System.Reflection;

namespace RAML.Api.Core
{
	public class ApiMultipleResponse : ApiMultipleObject
	{
		public void SetPropertyByStatusCode(string statusCode, object model)
		{
			var propName = string.Empty;
			
			if (names.ContainsKey(statusCode))
				propName = names[statusCode];

			if(names.ContainsKey("default"))
				propName = names["default"];

            GetType().GetTypeInfo().DeclaredProperties.First(p => p.Name == propName).SetValue(this, model);
		}

		public static string GetValueAsString(HttpStatusCode statusCode)
		{
			var val = (int)statusCode;
			return val.ToString();
		}
	}
}