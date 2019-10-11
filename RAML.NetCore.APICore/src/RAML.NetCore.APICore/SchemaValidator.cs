
using Newtonsoft.Json.Schema;
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
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.OrdinalIgnoreCase))
            {
                return new SchemaValidationResults(true, new List<string>());
            }

            var rawContent = await content.ReadAsStringAsync();
            return IsValidJSON(rawSchema, rawContent);
        }

        public static SchemaValidationResults IsValid(string rawSchema, HttpContent content)
        {
            if (content.Headers.ContentType == null || !content.Headers.ContentType.MediaType.Equals("application/json",
                StringComparison.OrdinalIgnoreCase))
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
                // TODO: check v4 schema validation                
            }

            return new SchemaValidationResults(true, new List<string>());
        }
    }
}
