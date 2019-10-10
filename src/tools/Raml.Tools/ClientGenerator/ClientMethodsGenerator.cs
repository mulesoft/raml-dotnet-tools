using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using Raml.Common;
using Raml.Parser.Expressions;
using Raml.Tools.WebApiGenerator;

namespace Raml.Tools.ClientGenerator
{
    public class ClientMethodsGenerator : MethodsGeneratorBase
    {
        private readonly IDictionary<string, ApiObject> uriParameterObjects;
        private readonly IDictionary<string, ApiObject> queryObjects;
        private readonly IDictionary<string, ApiObject> headerObjects;
        private readonly IDictionary<string, ApiObject> responseHeadersObjects;
        private readonly string defaultHeaderType = typeof(HttpResponseHeaders).Name;
        private readonly QueryParametersParser queryParametersParser;

        public ClientMethodsGenerator(RamlDocument raml, IDictionary<string, ApiObject> schemaResponseObjects, 
            IDictionary<string, ApiObject> uriParameterObjects, IDictionary<string, ApiObject> queryObjects, 
            IDictionary<string, ApiObject> headerObjects, IDictionary<string, ApiObject> responseHeadersObjects,
            IDictionary<string, ApiObject> schemaRequestObjects, IDictionary<string, string> linkKeysWithObjectNames,
            IDictionary<string, ApiObject> schemaObjects )
            : base(raml, schemaResponseObjects, schemaRequestObjects, linkKeysWithObjectNames, schemaObjects)
        {
            this.uriParameterObjects = uriParameterObjects;
            this.queryObjects = queryObjects;
            this.headerObjects = headerObjects;
            this.responseHeadersObjects = responseHeadersObjects;
            queryParametersParser = new QueryParametersParser(schemaObjects);
        }

        public ICollection<ClientGeneratorMethod> GetMethods(Resource resource, string url, ClassObject parent, string objectName, IDictionary<string, Parameter> parentUriParameters)
        {
            var methodsNames = new List<string>();
            if (parent != null)
                methodsNames = parent.Methods.Select(m => m.Name).ToList();

            var generatorMethods = new Collection<ClientGeneratorMethod>();
            if (resource.Methods == null)
                return generatorMethods;

            foreach (var method in resource.Methods)
            {
                AddGeneratedMethod(resource, url, objectName, method, methodsNames, generatorMethods, parentUriParameters);
            }

            return generatorMethods;
        }

        private void AddGeneratedMethod(Resource resource, string url, string objectName, Method method, ICollection<string> methodsNames, 
            ICollection<ClientGeneratorMethod> generatorMethods, IDictionary<string, Parameter> parentUriParameters)
        {
            var generatedMethod = BuildClassMethod(url, method, resource);
            if (generatedMethod.ReturnType != "string")
            {
                var returnType = CollectionTypeHelper.GetBaseType(generatedMethod.ReturnType);

                var returnTypeObject = schemaObjects.Values.Any(o => o.Name == returnType)
                    ? schemaObjects.Values.First(o => o.Name == returnType)
                    : schemaResponseObjects.Values.FirstOrDefault(o => o.Name == returnType);

                if (returnTypeObject != null)
                {
                    generatedMethod.ReturnTypeObject = returnTypeObject;
                    generatedMethod.OkReturnType = GetOkReturnType(generatedMethod);
                }

            }
            uriParametersGenerator.Generate(resource, url, generatedMethod, uriParameterObjects, parentUriParameters);

            if (!IsVerbForMethod(method)) return;

            if (methodsNames.Contains(generatedMethod.Name))
                generatedMethod.Name = GetUniqueName(methodsNames, generatedMethod.Name, resource.RelativeUri);

            GetQueryParameters(objectName, method, generatedMethod);

            GetHeaders(objectName, method, generatedMethod);

            GetResponseHeaders(objectName, generatedMethod, method);

            generatorMethods.Add(generatedMethod);
            methodsNames.Add(generatedMethod.Name);
        }

        private string GetResourceType(IDictionary<string, IDictionary<string, string>> type)
        {
            return type != null && type.Any() ? type.First().Key : string.Empty;
        }

        private ClientGeneratorMethod BuildClassMethod(string url, Method method, Resource resource)
        {
            var parentUrl = UrlGeneratorHelper.GetParentUri(url, resource.RelativeUri);

            var generatedMethod = new ClientGeneratorMethod
            {
                Name = NetNamingMapper.GetMethodName(method.Verb ?? "Get" + resource.RelativeUri),
                ReturnType = GetReturnType(GeneratorServiceHelper.GetKeyForResource(method, resource, parentUrl), method, resource, url),
                Parameter = GetParameter(GeneratorServiceHelper.GetKeyForResource(method, resource, parentUrl), method, resource, url),
                Comment = GetComment(resource, method, url),
                Url = url,
                Verb = NetNamingMapper.Capitalize(method.Verb),
                Parent = null,
                UseSecurity =
                    raml.SecuredBy != null && raml.SecuredBy.Any() ||
                    resource.Methods.Any(m => m.Verb == method.Verb && m.SecuredBy != null && m.SecuredBy.Any()),
                RequestContentTypes = method.Body.Keys.ToArray(),
                ResponseContentTypes = method.Responses != null ? method.Responses.Where(r => r.Body != null).SelectMany(r => r.Body.Keys).ToArray() : new string[0]
            };

            // look in traits 
            
            // look in resource types

            return generatedMethod;
        }

        private static string GetOkReturnType(ClientGeneratorMethod generatedMethod)
        {
            if (!generatedMethod.ReturnTypeObject.IsMultiple)
                return generatedMethod.ReturnType;

            if (generatedMethod.ReturnTypeObject.Properties.Any(p => p.StatusCode == HttpStatusCode.OK))
                return generatedMethod.ReturnTypeObject.Properties.First(p => p.StatusCode == HttpStatusCode.OK).Type;

            return generatedMethod.ReturnTypeObject.Properties.First().Type;
        }

        private void GetQueryParameters(string objectName, Method method, ClientGeneratorMethod generatedMethod)
        {
            if (method.QueryParameters != null && method.QueryParameters.Any())
            {
                var queryObject = queryParametersParser.GetQueryObject(generatedMethod, method, objectName);
                generatedMethod.Query = queryObject;
                if (!queryObjects.ContainsKey(queryObject.Name))
                    queryObjects.Add(queryObject.Name, queryObject);
            }
        }

        private void GetHeaders(string objectName, Method method, ClientGeneratorMethod generatedMethod)
        {
            if (method.Headers != null && method.Headers.Any())
            {
                var headerObject = HeadersParser.GetHeadersObject(generatedMethod, method, objectName);
                generatedMethod.Header = headerObject;
                headerObjects.Add(headerObject.Name, headerObject);
            }
        }

        private void GetResponseHeaders(string objectName, ClientGeneratorMethod generatedMethod, Method method)
        {
            generatedMethod.ResponseHeaders = new Dictionary<HttpStatusCode, ApiObject>();
            foreach (var resp in method.Responses.Where(r => r.Headers != null && r.Headers.Any()))
            {
                var headerObject = HeadersParser.GetHeadersObject(generatedMethod, resp, objectName);
                generatedMethod.ResponseHeaders.Add(ParserHelpers.GetHttpStatusCode(resp.Code), headerObject);
                responseHeadersObjects.Add(headerObject.Name, headerObject);
            }

            if (!generatedMethod.ResponseHeaders.Any())
            {
                generatedMethod.ResponseHeaderType = defaultHeaderType;
            }
            else if (generatedMethod.ResponseHeaders.Count == 1)
            {
                generatedMethod.ResponseHeaderType = ClientGeneratorMethod.ModelsNamespacePrefix + generatedMethod.ResponseHeaders.First().Value.Name;
            }
            else
            {
                CreateMultipleType(generatedMethod);
            }
        }

        private void CreateMultipleType(ClientGeneratorMethod generatedMethod)
        {
            var properties = BuildProperties(generatedMethod);

            var name = NetNamingMapper.GetObjectName("Multiple" + generatedMethod.Name + "Header");

            var apiObject = new ApiObject
            {
                Name = name,
                Description = "Multiple Header Types " + string.Join(", ", properties.Select(p => p.Name)),
                Properties = properties,
                IsMultiple = true
            };
            responseHeadersObjects.Add(new KeyValuePair<string, ApiObject>(name, apiObject));

            generatedMethod.ResponseHeaderType = ClientGeneratorMethod.ModelsNamespacePrefix + name;
        }

        private static List<Property> BuildProperties(ClientGeneratorMethod generatedMethod)
        {
            var properties = generatedMethod.ResponseHeaders
                .Select(kv => new Property
                              {
                                  Name = kv.Value.Name,
                                  Description = kv.Value.Description,
                                  Example = kv.Value.Example,
                                  StatusCode = kv.Key,
                                  Type = kv.Value.Name
                              })
                .ToList();
            return properties;
        }
    }
}