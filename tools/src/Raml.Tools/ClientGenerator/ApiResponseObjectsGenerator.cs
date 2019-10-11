using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Raml.Tools.ClientGenerator
{
    public class ApiResponseObjectsGenerator
    {
        public IEnumerable<ApiObject> Generate(IEnumerable<ClassObject> classObjects)
        {
            var objects = new Collection<ApiObject>();
            foreach (var classObject in classObjects)
            {
                foreach (var method in classObject.Methods)
                {
                    BuildResponse(method, classObject, objects);
                }
            }
            return objects;
        }

        private void BuildResponse(ClientGeneratorMethod method, ClassObject classObject, ICollection<ApiObject> objects)
        {
            var responseProperties = BuildResponseProperties(method).ToList();
            if (responseProperties.Any())
            {
                var respObject = new ApiObject
                {
                    Name = classObject.Name + method.Name + "Response",
                    Description = "Response object for method " + method.Name + " of class " + classObject.Name,
                    Properties = responseProperties,
                    IsUnionType = method.ReturnTypeObject != null && method.ReturnTypeObject.IsUnionType
                };
                objects.Add(respObject);
                method.ResponseType = ClientGeneratorMethod.ModelsNamespacePrefix + respObject.Name;
            }
            else
            {
                method.ResponseType = "ApiResponse";
            }
        }

        private IEnumerable<Property> BuildResponseProperties(ClientGeneratorMethod method)
        {
            var properties = new Collection<Property>();

            if (method.ReturnType != "string" && method.ReturnType != "HttpContent")
            {
                properties.Add(new Property
                {
                    Name = "Content",
                    Description = "Typed Response content",
                    Type = method.ReturnType
                });
            }

            if (method.ResponseHeaders != null && method.ResponseHeaders.Any())
            {
                properties.Add(new Property
                {
                    Name = "Headers",
                    Description = "Typed Response headers (defined in RAML)",
                    Type = method.ResponseHeaderType
                });
            }

            return properties;
        }
         
    }
}