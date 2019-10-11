using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Raml.Common;
#if PORTABLE
using System.Reflection;
#endif

namespace RAML.Api.Core
{
	public class ApiResponseHeader
	{
		public void SetProperties(HttpResponseHeaders headers)
		{
            var properties = this.GetType().GetTypeInfo().DeclaredProperties;

			foreach (var prop in properties.Where(prop => headers.Any(h => h.Key == prop.Name)))
			{
			    var value = headers.First(h => NetNamingMapper.GetPropertyName(h.Key) == prop.Name).Value;
                if(value == null)
                    continue;

			    if (value.Count() == 1)
			    {
			        prop.SetValue(this, value.FirstOrDefault());
			    }
                else
			    {
			        try
			        {
                        prop.SetValue(this, value.FirstOrDefault());
			        }
			        catch (Exception)
			        {
                        // do nothing
			        }
			    }
			}
		}
	}
}