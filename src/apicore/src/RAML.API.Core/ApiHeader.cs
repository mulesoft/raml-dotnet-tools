using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
#if PORTABLE
using System.Reflection;
#endif

namespace RAML.Api.Core
{
	public class ApiHeader
	{
		public IDictionary<string, string> Headers
		{
			get
			{
#if !PORTABLE
                var properties = this.GetType().GetProperties().Where(p => p.Name != "Headers" && p.GetValue(this) != null);
#else
                var properties = this.GetType().GetTypeInfo().DeclaredProperties.Where(p => p.Name != "Headers" && p.GetValue(this) != null);
#endif
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