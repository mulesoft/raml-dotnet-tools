using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AMF.Api.Core
{
	public class ApiHeader
	{
		public IDictionary<string, string> Headers
		{
			get
			{

                var properties = this.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.Name != "Headers" && p.GetValue(this) != null);
				return properties.ToDictionary(prop => GetKey(prop), prop => prop.GetValue(this).ToString());
			}
		}

        private string GetKey(PropertyInfo prop)
        {
            var jsonProp = prop.GetCustomAttribute<JsonPropertyAttribute>();
            if (jsonProp != null && !string.IsNullOrWhiteSpace(jsonProp.PropertyName))
                return jsonProp.PropertyName;

            return prop.Name;
        }
    }
}