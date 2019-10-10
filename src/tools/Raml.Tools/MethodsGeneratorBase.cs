using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Raml.Common;
using Raml.Parser.Expressions;
using Raml.Tools.Pluralization;

namespace Raml.Tools
{
    public abstract class MethodsGeneratorBase
    {
        protected readonly string[] suffixes = { "A", "B", "C", "D", "E", "F", "G" };
        protected readonly UriParametersGenerator uriParametersGenerator;

        protected readonly RamlDocument raml;
        protected readonly IDictionary<string, ApiObject> schemaObjects;
        protected readonly IDictionary<string, ApiObject> schemaResponseObjects;

        private ResponseTypesService responseTypesService;
        private RequestTypesService requestTypesService;

        protected MethodsGeneratorBase(RamlDocument raml, IDictionary<string, ApiObject> schemaResponseObjects,
            IDictionary<string, ApiObject> schemaRequestObjects, IDictionary<string, string> linkKeysWithObjectNames, IDictionary<string, ApiObject> schemaObjects)
        {
            this.raml = raml;
            this.schemaResponseObjects = schemaResponseObjects;
            this.schemaObjects = schemaObjects;
            responseTypesService = new ResponseTypesService(schemaObjects, schemaResponseObjects, linkKeysWithObjectNames, raml.ResourceTypes);
            requestTypesService = new RequestTypesService(schemaObjects, schemaRequestObjects, linkKeysWithObjectNames, raml.ResourceTypes);
            uriParametersGenerator = new UriParametersGenerator(schemaObjects);
        }

        protected string GetReturnType(string key, Method method, Resource resource, string fullUrl)
        {
            if (method.Responses.All(r => r.Body == null || r.Body.All(b => string.IsNullOrWhiteSpace(b.Value.Schema) && string.IsNullOrWhiteSpace(b.Value.Type))))
                return "string";

            var responses = method.Responses
                .Where(r => r.Body != null && r.Body.Any(b => !string.IsNullOrWhiteSpace(b.Value.Schema) || !string.IsNullOrWhiteSpace(b.Value.Type)))
                .ToArray();

            var returnType = HandleMultipleSchemaType(responses, resource, method, key, fullUrl);

            if (!string.IsNullOrWhiteSpace(returnType))
                return returnType;

            return "string";
        }

        private string HandleMultipleSchemaType(IEnumerable<Response> responses, Resource resource, Method method, string key, string fullUrl)
        {
            var properties = GetProperties(responses, resource, method, key, fullUrl);

            if (properties.Count == 0)
                return "string";

            if (properties.Count == 1)
                return properties.First().Type;

            // Build a new response object containing all types
            var name = NetNamingMapper.GetObjectName("Multiple" + key);
            var apiObject = new ApiObject
            {
                Name = name,
                Description = "Multiple Response Types " + string.Join(", ", properties.Select(p => p.Name)),
                Properties = properties,
                IsMultiple = true
            };
            schemaResponseObjects.Add(new KeyValuePair<string, ApiObject>(name, apiObject));
            return name;
        }

        private List<Property> GetProperties(IEnumerable<Response> responses, Resource resource, Method method, string key, string fullUrl)
        {
            var properties = new List<Property>();
            foreach (var response in responses)
            {
                AddProperty(resource, method, key, response, properties, fullUrl);
            }
            return properties;
        }

        private void AddProperty(Resource resource, Method method, string key, Response response, ICollection<Property> properties, string fullUrl)
        {
            var mimeType = GeneratorServiceHelper.GetMimeType(response);
            if (mimeType == null)
                return;

            var type = responseTypesService.GetResponseType(method, resource, mimeType, key, response.Code, fullUrl);
            if (string.IsNullOrWhiteSpace(type))
                return;

            var name = NetNamingMapper.GetPropertyName(CollectionTypeHelper.GetBaseType(type));
            if (properties.Any(p => p.Name == name))
                name = name + response.Code;

            var property = new Property
                           {
                               Name = name,
                               Description = response.Description + " " + mimeType.Description,
                               Example = mimeType.Example,
                               Type = type,
                               StatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), response.Code),
                               JSONSchema = mimeType.Schema == null ? null : mimeType.Schema.Replace(Environment.NewLine, "").Replace("\r\n", "").Replace("\n", "").Replace("\"", "\\\"")
                           };

            properties.Add(property);
        }

        protected string GetComment(Resource resource, Method method, string url)
        {
            var description = resource.Description;
            if (!string.IsNullOrWhiteSpace(method.Description))
                description += string.IsNullOrWhiteSpace(description) ? method.Description : ". " + method.Description;

            if(description != null)
                description = new SchemaParameterParser(new EnglishPluralizationService()).Parse(description, resource, method, url);

            description = ParserHelpers.RemoveNewLines(description);

            if (!string.IsNullOrWhiteSpace(resource.DisplayName))
                description += string.IsNullOrWhiteSpace(description) ? resource.DisplayName : " - " + resource.DisplayName;

            return description;
        }

        protected GeneratorParameter GetParameter(string key, Method method, Resource resource, string fullUrl)
        {
            return requestTypesService.GetRequestParameter(key, method, resource, fullUrl, raml.MediaType);
		}




        protected bool IsVerbForMethod(Method method)
        {
            if (method.Verb == null)
                return true;

            return method.Verb.ToLower() != "options" && method.Verb.ToLower() != "head" && method.Verb.ToLower() != "trace" && method.Verb.ToLower() != "connect";
        }

        protected string GetUniqueName(ICollection<string> methodsNames, string methodName, string relativeUri)
        {
            var nameWithResource = NetNamingMapper.GetMethodName(methodName + relativeUri);
            if (!methodsNames.Contains(nameWithResource))
                return nameWithResource;

            for (var i = 0; i < 7; i++)
            {
                var unique = methodName + suffixes[i];
                if (!methodsNames.Contains(unique))
                    return unique;
            }
            for (var i = 0; i < 100; i++)
            {
                var unique = methodName + i;
                if (!methodsNames.Contains(unique))
                    return unique;
            }
            throw new InvalidOperationException("Could not find a unique name for method " + methodName);
        }

    }
}