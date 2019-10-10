using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Raml.Tools.ClientGenerator
{
    public class ApiRequestObjectsGenerator
    {
        public IEnumerable<ApiObject> Generate(IEnumerable<ClassObject> classObjects)
        {
            var objects = new Collection<ApiObject>();
            foreach (var classObject in classObjects)
            {
                foreach (var method in classObject.Methods)
                {
                    BuildRequest(method, classObject, objects);
                }
            }
            return objects;
        }

        private void BuildRequest(ClientGeneratorMethod method, ClassObject classObject, ICollection<ApiObject> objects)
        {
            var requestProperties = BuildRequestProperties(method).ToList();
            if (requestProperties.Any())
            {
                var reqObject = new ApiObject
                {
                    Name = classObject.Name + method.Name + "Request",
                    Description = "Request object for method " + method.Name + " of class " + classObject.Name,
                    Properties = requestProperties
                };
                objects.Add(reqObject);
                method.RequestType = ClientGeneratorMethod.ModelsNamespacePrefix + reqObject.Name;
            }
            else
            {
                method.RequestType = "ApiRequest";
            }
        }

        private IEnumerable<Property> BuildRequestProperties(ClientGeneratorMethod method)
        {
            var properties = new Collection<Property>();

            if (method.Header != null)
            {
                properties.Add(new Property
                {
                    Name = "Headers",
                    Description = "Typed Request headers",
                    Type = method.Header.Name
                });
            }

            if (method.Verb.ToLowerInvariant() != "get" && method.Verb.ToLowerInvariant() != "delete")
            {
                properties.Add(new Property
                {
                    Name = "Content",
                    Description = "Request content",
                    Type = method.Parameter != null && method.Parameter.Type != "string" ? method.Parameter.Type : "HttpContent"
                });

                properties.Add(new Property
                               {
                                   Name = "Formatter",
                                   Description = "Request formatter",
                                   Type = "MediaTypeFormatter"
                               });
            }

            if (method.Query != null)
            {
                properties.Add(new Property
                {
                    Name = "Query",
                    Description = "Request query string properties",
                    Type = method.Query.Name
                });
            }

            if (method.UriParameters.Any())
            {
                properties.Add(new Property
                {
                    Name = "UriParameters",
                    Description = "Request Uri Parameters",
                    Type = method.UriParametersType
                });
            }

            return properties;
        }


    }
}