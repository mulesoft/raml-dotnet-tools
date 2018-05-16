using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AMF.Api.Core;
using AMF.Parser.Model;
using AMF.Tools.Core.Pluralization;

namespace AMF.Tools.Core
{
    public abstract class MethodsGeneratorBase
    {
        protected readonly string[] suffixes = { "A", "B", "C", "D", "E", "F", "G" };
        protected readonly UriParametersGenerator uriParametersGenerator;

        protected readonly AmfModel raml;
        protected readonly IDictionary<string, ApiObject> schemaObjects;
        protected readonly IDictionary<string, ApiObject> schemaResponseObjects;

        private ResponseTypesService responseTypesService;
        private RequestTypesService requestTypesService;

        protected MethodsGeneratorBase(AmfModel raml, IDictionary<string, ApiObject> schemaResponseObjects,
            IDictionary<string, ApiObject> schemaRequestObjects, IDictionary<string, string> linkKeysWithObjectNames, IDictionary<string, ApiObject> schemaObjects)
        {
            this.raml = raml;
            this.schemaResponseObjects = schemaResponseObjects;
            this.schemaObjects = schemaObjects;
            responseTypesService = new ResponseTypesService(schemaObjects, schemaResponseObjects, linkKeysWithObjectNames);
            requestTypesService = new RequestTypesService(schemaObjects, schemaRequestObjects, linkKeysWithObjectNames);
            uriParametersGenerator = new UriParametersGenerator(schemaObjects);
        }

        protected string GetReturnType(string key, Operation method, Parser.Model.EndPoint resource, string fullUrl)
        {
            if (method.Responses.All(r => !r.Payloads.Any(p => p.Schema != null)))
                return "string";

            var responses = method.Responses
                .Where(r => r.Payloads.Any(b => b.Schema != null))
                .ToArray();

            var returnType = HandleMultipleSchemaType(responses, resource, method, key, fullUrl);

            if (!string.IsNullOrWhiteSpace(returnType))
                return returnType;

            return "string";
        }

        private string HandleMultipleSchemaType(IEnumerable<Response> responses, Parser.Model.EndPoint resource, Operation method, string key, string fullUrl)
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

        private List<Property> GetProperties(IEnumerable<Response> responses, Parser.Model.EndPoint resource, Operation method, string key, string fullUrl)
        {
            var properties = new List<Property>();
            foreach (var response in responses)
            {
                AddProperty(resource, method, key, response, properties, fullUrl);
            }
            return properties;
        }

        private void AddProperty(Parser.Model.EndPoint resource, Operation method, string key, Response response, ICollection<Property> properties, string fullUrl)
        {
            var mimeType = GeneratorServiceHelper.GetMimeType(response);
            if (mimeType == null)
                return;

            var type = responseTypesService.GetResponseType(method, resource, mimeType, key, response.StatusCode, fullUrl);
            if (string.IsNullOrWhiteSpace(type))
                return;

            var name = NetNamingMapper.GetPropertyName(CollectionTypeHelper.GetBaseType(type));
            if (properties.Any(p => p.Name == name))
                name = name + response.StatusCode;

            var property = new Property
                           {
                               Name = name,
                               Description = response.Description + " " + mimeType.Schema.Description,
                               Example = ObjectParser.MapExample(mimeType.Schema),
                               Type = type,
                               StatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), response.StatusCode),
                               JSONSchema = mimeType.Schema as SchemaShape == null ? null : ((SchemaShape)mimeType.Schema).Raw.Replace(Environment.NewLine, "").Replace("\r\n", "").Replace("\n", "").Replace("\"", "\\\"")
                           };

            properties.Add(property);
        }

        protected string GetComment(Parser.Model.EndPoint resource, Operation method, string url)
        {
            var description = resource.Description;
            if (!string.IsNullOrWhiteSpace(method.Description))
                description += string.IsNullOrWhiteSpace(description) ? method.Description : ". " + method.Description;

            if(description != null)
                description = new SchemaParameterParser(new EnglishPluralizationService()).Parse(description, resource, method, url);

            description = ParserHelpers.RemoveNewLines(description);

            if (!string.IsNullOrWhiteSpace(resource.Path))
                description += string.IsNullOrWhiteSpace(description) ? resource.Path : " - " + resource.Path;

            return description;
        }

        protected GeneratorParameter GetParameter(string key, Operation method, Parser.Model.EndPoint resource, string fullUrl)
        {
            return requestTypesService.GetRequestParameter(key, method, resource, fullUrl, raml.WebApi.Accepts);
		}

        protected bool IsVerbForMethod(Operation operation)
        {
            if (operation.Method == null)
                return true;

            return operation.Method.ToLower() != "options" && operation.Method.ToLower() != "head" 
                && operation.Method.ToLower() != "trace" && operation.Method.ToLower() != "connect";
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