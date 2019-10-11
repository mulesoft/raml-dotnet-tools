using System.Linq;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public class GeneratorServiceHelper
    {
        public static MimeType GetMimeType(Response response)
        {
            if (!response.Body.Any(b => b.Value != null && (!string.IsNullOrWhiteSpace(b.Value.Schema) || !string.IsNullOrWhiteSpace(b.Value.Type)) ))
                return null;

            MimeType mimeType;
            if (response.Body.Any(b => b.Value != null && (!string.IsNullOrWhiteSpace(b.Value.Schema) || !string.IsNullOrWhiteSpace(b.Value.Type)) && b.Key == "application/json"))
            {
                mimeType = response.Body.First(b => b.Value != null
                                                    && (!string.IsNullOrWhiteSpace(b.Value.Schema) || !string.IsNullOrWhiteSpace(b.Value.Type))
                                                    && b.Key == "application/json").Value;
            }
            else
            {
                mimeType = response.Body.First(b => b.Value != null && (!string.IsNullOrWhiteSpace(b.Value.Schema) || !string.IsNullOrWhiteSpace(b.Value.Type))).Value;
            }
            return mimeType;
        }

        public static string GetKeyForResource(Method method, Resource resource, string parentUrl)
        {
            return parentUrl + resource.RelativeUri + "-" + (string.IsNullOrWhiteSpace(method.Verb) ? "Get" : method.Verb);
        }

    }
}