using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using RAML.Api.Core;
using RAML.Parser.Model;
using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Tools.Core
{
    public class WebApiMethodsGenerator : MethodsGeneratorBase
    {
        private readonly QueryParametersParser queryParametersParser;

        public WebApiMethodsGenerator(AmfModel raml, IDictionary<string, ApiObject> schemaResponseObjects, 
            IDictionary<string, ApiObject> schemaRequestObjects, IDictionary<string, string> linkKeysWithObjectNames, 
            IDictionary<string, ApiObject> schemaObjects, IDictionary<string, ApiEnum> enums)
            : base(raml, schemaResponseObjects, schemaRequestObjects, linkKeysWithObjectNames, schemaObjects, enums)
        {
            queryParametersParser = new QueryParametersParser(schemaObjects);
        }

        public IEnumerable<ControllerMethod> GetMethods(RAML.Parser.Model.EndPoint endpoint, string url, ControllerObject parent, string objectName, 
            IDictionary<string, Parameter> parentUriParameters, string modelsNamespace)
        {
            var methodsNames = new List<string>();
            if (parent != null && parent.Methods != null)
                methodsNames = parent.Methods.Select(m => m.Name).ToList();

            var generatorMethods = new Collection<ControllerMethod>();
            if (endpoint.Operations == null)
                return generatorMethods;

            foreach (var method in endpoint.Operations)
            {
                var generatedMethod = BuildControllerMethod(url, method, endpoint, parent, parentUriParameters, modelsNamespace);

                if (IsVerbForMethod(method))
                {
                    if (methodsNames.Contains(generatedMethod.Name))
                        generatedMethod.Name = GetUniqueName(methodsNames, generatedMethod.Name, GetRelativePath(endpoint.Path, parent.PrefixUri));

                    if (method.Request != null && method.Request.QueryParameters != null && method.Request.QueryParameters.Any())
                    {
                        var queryParameters = queryParametersParser.ParseParameters(method);
                        generatedMethod.QueryParameters = queryParameters;
                    }

                    generatorMethods.Add(generatedMethod);
                    methodsNames.Add(generatedMethod.Name);
                }
            }

            return generatorMethods;
        }

        private string GetRelativePath(string path, string prefixUri)
        {
            if (string.IsNullOrWhiteSpace(prefixUri))
                return path;

            return path.Replace(prefixUri, string.Empty);
        }

        private ControllerMethod BuildControllerMethod(string url, Operation method, RAML.Parser.Model.EndPoint resource, ControllerObject parent, 
            IDictionary<string, Parameter> parentUriParameters, string modelsNamespace)
        {
            var relativeUri = UrlGeneratorHelper.GetRelativeUri(url, parent.PrefixUri);

            var parentUrl = UrlGeneratorHelper.GetParentUri(url, resource.Path);

            var operationWithSecurity = resource.Operations.FirstOrDefault(m => m.Method == method.Method && m.Security != null
                                && m.Security.Any());
            var sec = operationWithSecurity?.Security?.Where(s => s != null);
            var securedBy = sec?.SelectMany(se => se.Schemes?.Where(s => s != null))
                .Select(s => s.Name).ToArray();

            return new ControllerMethod
            {
                ModelsNamespace = modelsNamespace,
                Name = NetNamingMapper.GetMethodName(method.Method ?? "Get" + resource.Path),
                Parameter = GetParameter(GeneratorServiceHelper.GetKeyForResource(method, resource), method, resource, url),
                UriParameters = uriParametersGenerator.GetUriParameters(resource, url, parentUriParameters),
                ResponseStatusCode = method.Responses != null && method.Responses.Count() == 1 ? method.Responses.First().StatusCode : null,
                ReturnType = GetReturnType(method, resource, url),
                Comment = GetComment(resource, method, url),
                Url = relativeUri,
                Verb = NetNamingMapper.Capitalize(method.Method),
                Parent = null,
                UseSecurity = resource.Operations.Any(m => m.Method == method.Method && m.Security != null && m.Security.Any()),
                SecuredBy = securedBy,
                SecurityParameters = GetSecurityParameters(method)
            };
        }

        private IEnumerable<Property> GetSecurityParameters(Operation method)
        {
            var securityParams = new Collection<Property>();

            var securedBy = method.Security != null && method.Security.Any() ? method.Security : null;

            if (securedBy == null)
                return securityParams;

            var secReq = securedBy.FirstOrDefault(s => s != null);
            if(secReq == null)
                return securityParams;

            var secured = secReq.Schemes?.FirstOrDefault(s => s != null);
            if(secured == null || secured.SecurityScheme == null)
                return securityParams;

            return queryParametersParser.ConvertParametersToProperties(secured.SecurityScheme.QueryParameters);
        }
    }
}