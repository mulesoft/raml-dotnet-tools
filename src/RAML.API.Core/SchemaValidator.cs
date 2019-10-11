
using Newtonsoft.Json.Schema;
using v4SchemaNS = Newtonsoft.JsonV4.Schema;
using v4LinqNS = Newtonsoft.JsonV4.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RAML.Api.Core
{
    public static class SchemaValidator
    {
        public static async Task ValidateWithExceptionAsync(string rawSchema, HttpContent content)
        {
            var results = await IsValidAsync(rawSchema, content);
            if (!results.IsValid)
            {
                throw new SchemaValidationException("The response is not valid according to the associated schema", results.Errors);
            }
        }

        public static async Task<SchemaValidationResults> IsValidAsync(string rawSchema, HttpContent content)
        {
#if !PORTABLE
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.InvariantCultureIgnoreCase))
#else
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.OrdinalIgnoreCase))
#endif
            {
                return new SchemaValidationResults(true, new List<string>());
            }

            var rawContent = await content.ReadAsStringAsync();
            return IsValidJSON(rawSchema, rawContent);
        }

        public static SchemaValidationResults IsValid(string rawSchema, HttpContent content)
        {
#if !PORTABLE
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.InvariantCultureIgnoreCase))
#else
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.OrdinalIgnoreCase))
#endif
            {
                return new SchemaValidationResults(true, new List<string>());
            }

            var readTask = content.ReadAsStringAsync().ConfigureAwait(false);
            var rawResponse = readTask.GetAwaiter().GetResult();

            return IsValidJSON(rawSchema, rawResponse);

        }

        private static SchemaValidationResults IsValidJSON(string rawSchema, string responseString)
        {
            JsonSchema schema;
            v4SchemaNS.JsonSchema v4Schema;
            
            JToken data = null;
            if (responseString.StartsWith("["))
                data = JArray.Parse(responseString);
            else
                data = JObject.Parse(responseString);

            IList<string> errors;

            try
            {
                schema = JsonSchema.Parse(rawSchema);

                if (!data.IsValid(schema, out errors))
                {
                    return new SchemaValidationResults (false, errors);
                }
            }
            catch (Exception)
            {
                v4Schema = v4SchemaNS.JsonSchema.Parse(rawSchema);
                
                // Definitions are not supported
                if(v4Schema.Definitions != null && v4Schema.Definitions.Any())
                    return new SchemaValidationResults(false, new[] { "Definitions are not supported. Don not use Schema Validation with schemas that contain definitions." });

                v4LinqNS.JToken datav4 = null;
                if (responseString.StartsWith("["))
                    datav4 = v4LinqNS.JArray.Parse(responseString);
                else
                    datav4 = v4LinqNS.JObject.Parse(responseString);
                                        
                if (!v4SchemaNS.Extensions.IsValid(datav4, v4Schema, out errors))
                {
                    return new SchemaValidationResults(false, errors);
                }
                
            }

            return new SchemaValidationResults(true, new List<string>());
        }
    }
}
