using System.Linq;
using RAML.Parser.Model;

namespace AMF.Tools.Core
{
    public class GeneratorServiceHelper
    {
        public static Payload GetMimeType(Response response)
        {
            if (!response.Payloads.Any(b => b.Schema != null))
                return null;

            var payload = response.Payloads.FirstOrDefault(p => p.MediaType == "application/json");

            return payload ?? response.Payloads.First(); //TODO: check
        }

        public static string GetKeyForResource(Operation operation, EndPoint resource)
        {
            return resource.Path + "-" + (string.IsNullOrWhiteSpace(operation.Method) ? "Get" : operation.Method);
        }

        public static string GetKeyForResource(Operation operation, EndPoint resource, Response response)
        {
            return resource.Path + "-" + (string.IsNullOrWhiteSpace(operation.Method) ? "Get" : operation.Method) + response.Name;
        }
    }
}