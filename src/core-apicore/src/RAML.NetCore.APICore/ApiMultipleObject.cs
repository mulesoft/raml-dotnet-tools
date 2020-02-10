using System.Collections.Generic;
using System.Net;

namespace RAML.Api.Core
{
    public class ApiMultipleObject
    {
        protected IDictionary<string, string> names = new Dictionary<string, string>();
        protected IDictionary<string, System.Type> types = new Dictionary<string, System.Type>();

        public System.Type GetTypeByStatusCode(string statusCode)
        {
            if (types.ContainsKey(statusCode))
                return types[statusCode];

            return null;
        }
 
    }
}