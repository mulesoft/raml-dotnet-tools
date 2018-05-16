using AMF.Parser.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using RAML.Api.Core;
using System.Collections.Specialized;

namespace RAML.WebApiExplorer
{
	public abstract class ApiExplorerService
	{
        public enum RamlVersion
        {
            Version08,
            Version1
        }

        private readonly IApiExplorer apiExplorer;
		private readonly string host;
        private readonly string basePath;
        private SecurityScheme securityScheme;
		private string securityType;

		public IEnumerable<string> SecuredBy { get; set; }

		public IEnumerable<ParametrizedSecurityScheme> SecuritySchemes { get; set; }

		public IEnumerable<string> Protocols { get; set; }

        public string DefaultMediaType { get; set; }

        protected readonly IDictionary<string, string> Schemas = new Dictionary<string, string>();
	    protected readonly IDictionary<Type, Shape> Types = new Dictionary<Type, Shape>();

	    public ApiExplorerService(IApiExplorer apiExplorer, string baseUri = null)
		{
			this.apiExplorer = apiExplorer;
			host = GetHost(baseUri);
            basePath = GetPath(baseUri);
		}

        private string GetPath(string baseUri)
        {
            if (string.IsNullOrWhiteSpace(baseUri))
                return null;

            try
            {
                var uri = new Uri(baseUri);
                return uri.AbsolutePath;
            }
            catch
            {
                return null;
            }
        }

        private string GetHost(string baseUri)
        {
            if (string.IsNullOrWhiteSpace(baseUri))
                return null;

            try
            {
                var uri = new Uri(baseUri);
                return uri.Host;
            }
            catch
            {
                return null;
            }
        }

        public WebApi GetRaml(RamlVersion ramlVersion = RamlVersion.Version1, string title = null)
		{
			if (string.IsNullOrWhiteSpace(title))
				title = "Api";

			
			var resourcesDic = new Dictionary<string, EndPoint>();
			var parameterDescriptionsDic = new Dictionary<string, Collection<ApiParameterDescription>>();
			foreach (var api in apiExplorer.ApiDescriptions)
			{
				var relativeUri = !api.Route.RouteTemplate.StartsWith("/") ? "/" + api.Route.RouteTemplate : api.Route.RouteTemplate;
				if (relativeUri.Contains("{controller}"))
				{
					relativeUri = relativeUri.Replace("{controller}", api.ActionDescriptor.ControllerDescriptor.ControllerName);

					if (relativeUri.EndsWith("/"))
						relativeUri = relativeUri.Substring(0, relativeUri.Length - 1);
				}

				foreach (var apiParam in GetParametersFromUrl(relativeUri))
				{
					relativeUri = RemoveNonExistingParametersFromRoute(relativeUri, api, apiParam.Name);
				}

				EndPoint resource;
				if (!resourcesDic.ContainsKey(relativeUri))
				{
                    IEnumerable<ParametrizedSecurityScheme> endpointSecurity = null;

                    var operations = GetMethods(api);

                    parameterDescriptionsDic.Add(relativeUri, api.ParameterDescriptions);
                    var parameters = GetParameters(api.RelativePath, parameterDescriptionsDic[api.RelativePath]);
                    
                    resource = new EndPoint(name: null, description: api.Documentation, path: api.RelativePath, operations: operations, 
                        parameters: parameters, security: endpointSecurity);

					resourcesDic.Add(relativeUri, resource);
				}
				else
				{
					resource = resourcesDic[relativeUri];
					foreach (var apiParameterDescription in api.ParameterDescriptions)
					{
						parameterDescriptionsDic[relativeUri].Add(apiParameterDescription);
					}
				}

                SetResourceProperties?.Invoke(resource, api);

                SetResourcePropertiesByAction?.Invoke(resource, api.ActionDescriptor);

                SetResourcePropertiesByController?.Invoke(resource, api.ActionDescriptor.ControllerDescriptor);
            }

            var endPoints = resourcesDic.Select(d => d.Value).OrderBy(e => e.Path).ToArray();
            var raml = new WebApi(name: title, description: null, host: host, schemes: Protocols, endPoints: endPoints, basePath: basePath, 
                accepts: null, contentType: null, version: null, termsOfService: null, provider: null, license: null, documentations: null, 
                baseUriParameters: null, security: SecuritySchemes);

            // raml = SetUriParameters(raml.EndPoints, parameterDescriptionsDic, string.Empty);

			if(SetRamlProperties != null)
                raml = SetRamlProperties(raml);

			//if (SecuritySchemes != null)
			//	securitySchemes = SecuritySchemes;
			
			//if (!string.IsNullOrWhiteSpace(securityType) && securityScheme != null)
   //             raml = SetSecurityScheme(raml);

			//if (SecuredBy != null)
			//	securedBy = SecuredBy;

			return raml;
		}

		public void SetSecurityScheme(string type, SecurityScheme scheme)
		{
			securityScheme = scheme;
			securityType = type;
		}

		public WebApi UseOAuth2(WebApi webApi, string authorizationUri, string accessTokenUri, IEnumerable<string> authorizationGrants, IEnumerable<Scope> scopes, SecurityScheme securitySchemeDescriptor)
		{
			securityType = "oauth_2_0";
            var settings = new Settings(null, authorizationUri, null, null, accessTokenUri, authorizationGrants, null, scopes, null, null);
            var security = new List<ParametrizedSecurityScheme> { new ParametrizedSecurityScheme("OAuth 2.0", securityScheme, settings) };

            return new WebApi(webApi.Name, webApi.Description, webApi.Host, webApi.Schemes, webApi.EndPoints, webApi.BasePath, webApi.Accepts,
                webApi.ContentType, webApi.Version, webApi.TermsOfService, webApi.Provider, webApi.License, webApi.Documentations,
                webApi.BaseUriParameters, security);
		}

		public WebApi UseOAuth1(WebApi webApi, string authorizationUri, string requestTokenUri, string tokenCredentialsUri, SecurityScheme securitySchemeDescriptor)
		{
			securityType = "oauth_1_0";
            var settings = new Settings(requestTokenUri, authorizationUri, tokenCredentialsUri, null, null, null, null, null, null, null);
            var security = new List<ParametrizedSecurityScheme> { new ParametrizedSecurityScheme("OAuth 1.0", securityScheme, settings) };

            return new WebApi(webApi.Name, webApi.Description, webApi.Host, webApi.Schemes, webApi.EndPoints, webApi.BasePath, webApi.Accepts,
                webApi.ContentType, webApi.Version, webApi.TermsOfService, webApi.Provider, webApi.License, webApi.Documentations,
                webApi.BaseUriParameters, security);
        }

        public Func<WebApi, WebApi> SetRamlProperties { get; set; }

		public Func<EndPoint, ApiDescription, WebApi> SetResourceProperties  { get; set; }

		public Func<EndPoint, HttpControllerDescriptor, WebApi> SetResourcePropertiesByController { get; set; }

		public Func<EndPoint, HttpActionDescriptor, WebApi> SetResourcePropertiesByAction { get; set; }

        public Func<ApiDescription, Operation, WebApi> SetMethodProperties { get; set; }

		private static string RemoveNonExistingParametersFromRoute(string relativeUri, ApiDescription api, string parameterName)
		{
			// if the parameter in the route is not a parameter in the method then it does not use it so I remove it from the route
			var parameterString = "{" + parameterName.ToLowerInvariant() + "}";
			if (relativeUri.Length > parameterString.Length + 1
			    && api.ParameterDescriptions.All(p => p.Name.ToLowerInvariant() != parameterName.ToLowerInvariant()))
			{
				relativeUri = relativeUri.Replace(parameterString, string.Empty);
			}

			return relativeUri;
		}

		//private void SetUriParameters(IEnumerable<EndPoint> resources, Dictionary<string, Collection<ApiParameterDescription>> parameterDescriptionsDic, 
  //          string parentUrl)
		//{
		//	if(resources == null)
		//		return;

		//	foreach (var resource in resources)
		//	{
  //              resource.Parameters = ;
		//	}
		//}

		private string GetDescription(ApiDescription api)
		{
			var description = string.Empty;
			
			if (!string.IsNullOrWhiteSpace(api.Documentation))
				description += api.Documentation;

			description += " (" + api.ActionDescriptor.ControllerDescriptor.ControllerName + "." + api.ActionDescriptor.ActionName + ")";

			return description;
		}

		private IEnumerable<Operation> GetMethods(ApiDescription api)
		{
			var methods = new Collection<Operation>();
			foreach (var httpMethod in api.ActionDescriptor.SupportedHttpMethods)
			{
                var queryParameters = GetQueryParameters(api.ParameterDescriptions);
                
                var method = new Operation(method: api.HttpMethod.Method, name: api.ActionDescriptor.ActionName, description: GetDescription(api), deprecated: false, 
                    summary: null, documentation: null, schemes: null, accepts: null, contentType: null, request: GetRequest(api), 
                    responses: GetResponses(api.ResponseDescription, api), security: null);

				methods.Add(method);

                SetMethodProperties?.Invoke(api, method);
            }
			return methods;
		}

		private IEnumerable<Response> GetResponses(ResponseDescription responseDescription, ApiDescription api)
		{
            var responses = new List<Response>();

			if (responseDescription.ResponseType == null && responseDescription.DeclaredType == null)
				return responses;

			var responseType = responseDescription.ResponseType ?? responseDescription.DeclaredType;

            var attributes = api.ActionDescriptor.GetCustomAttributes<Attribute>();
            responses = HandleResponseTypeStatusAttributes(attributes);

            if(responseType == typeof(IHttpActionResult))
                return responses;

            responses.Add(HandleResponseTypeAttributes(responseType));

			return responses;
		}

	    private Response HandleResponseTypeAttributes(Type responseType)
	    {
            var type = AddType(responseType);

            var body = CreateJsonMimeType(type.Name);
            return new Response(name: "200", description: null, statusCode: "200", headers: null, payloads: body, examples: null);
	    }

	    private List<Response> HandleResponseTypeStatusAttributes(IEnumerable<Attribute> attributes)
	    {
            var responses = new Dictionary<string,Response>();
            foreach (var attribute in attributes.Where(a => a is ResponseTypeStatusAttribute))
            {
                var response = GetResponse(attribute);
                if(!responses.ContainsKey(response.StatusCode))
                    responses.Add(response.StatusCode, response);
            }
	        return responses.Values.ToList();
	    }

	    private Response GetResponse(Attribute attribute)
	    {
            var status = ((ResponseTypeStatusAttribute)attribute).StatusCode;
            var type = ((ResponseTypeStatusAttribute)attribute).ResponseType;
            var typeName = AddType(type);
            return new Response(name: status.ToString(), description: null, statusCode: status.ToString(), headers: null, 
                payloads: CreateJsonMimeType(typeName.Name), examples: null);
	    }

        protected IEnumerable<Payload> CreateJsonMimeType(string type)
        {
            var mimeType = CreateMimeType(type);
            return CreateMimeTypes(mimeType);
        }

	    protected IEnumerable<Payload> CreateMimeTypes(Shape mimeType)
	    {
            var mimeTypes = new List<Payload>
            {
                new Payload("application/json", mimeType)
	        };
	        return mimeTypes;
	    }


	    protected abstract Shape AddType(Type type);

	    protected string GetUniqueSchemaName(string schemaName)
	    {
	        for (var i = 0; i < 1000; i++)
	        {
	            schemaName += i;
	            if (!Schemas.ContainsKey(schemaName))
	                return schemaName;
	        }
            throw new InvalidOperationException("Could not find a unique name. You have more than 1000 types with the same class name");
	    }

	    private Request GetRequest(ApiDescription api)
		{
			var mediaTypes = api.SupportedRequestBodyFormatters.SelectMany(f => f.SupportedMediaTypes).ToArray();

			var apiParam = api.ParameterDescriptions.FirstOrDefault(p => p.Source == ApiParameterSource.FromBody);
			Shape shape = null;

			if (apiParam != null)
			{
				var type = apiParam.ParameterDescriptor.ParameterType;
                shape = AddType(type);
			}

            var queryParams = GetQueryParameters(api.ParameterDescriptions).Select(p => p.Value).ToArray();
            var payloads = new List<Payload>();
            if (shape != null && mediaTypes.Any(m => m.MediaType == "application/json"))
                payloads.Add(new Payload("application/json", shape));

            Shape queryString = null; //TODO: check
            IEnumerable<Parameter> headers = null;  //TODO: check
            return new Request(queryParams, headers, payloads, queryString);
		}

	    protected abstract Shape CreateMimeType(string type);


	    private IDictionary<string, Parameter> GetQueryParameters(IEnumerable<ApiParameterDescription> parameterDescriptions)
        {
            var queryParams = new Dictionary<string, Parameter>();

            foreach (var apiParam in parameterDescriptions.Where(p => p.Source == ApiParameterSource.FromUri))
            {
                if (apiParam.ParameterDescriptor == null)
                    continue;

                if (!IsPrimitiveType(apiParam.ParameterDescriptor.ParameterType))
                {
                    GetParametersFromComplexType(apiParam, queryParams);
                }
                else
                {
                    var parameter = GetPrimitiveParameter(apiParam, "query");

                    if (!queryParams.ContainsKey(apiParam.Name))
                        queryParams.Add(apiParam.Name, parameter);
                }
            }
            return queryParams;
        }

	    private void GetParametersFromComplexType(ApiParameterDescription apiParam, IDictionary<string, Parameter> queryParams)
	    {
	        var properties = apiParam.ParameterDescriptor.ParameterType
	            .GetProperties().Where(p => p.CanWrite);
	        foreach (var property in properties)
	        {
                if(!IsPrimitiveType(property.PropertyType))
                    continue;

	            var parameter = GetParameterFromProperty(apiParam, property, "query");

	            if (!queryParams.ContainsKey(property.Name))
	                queryParams.Add(property.Name, parameter);
	        }
	    }

	    private Parameter GetParameterFromProperty(ApiParameterDescription apiParam, PropertyInfo property, string binding)
	    {
            var parameter = new Parameter(name: apiParam.Name, description: apiParam.Documentation, 
                required: !IsNullable(property.PropertyType), binding: binding, schema: AddType(property.PropertyType));
	        return parameter;
	    }

	    private Parameter GetPrimitiveParameter(ApiParameterDescription apiParam, string binding)
	    {
            var parameter = new Parameter(name: apiParam.Name, description: apiParam.Documentation, required: !apiParam.ParameterDescriptor.IsOptional,
                binding: binding, schema: AddType(apiParam.ParameterDescriptor.ParameterType));
	        return parameter;
	    }

	    private static bool IsNullable(Type type)
	    {
            return type == typeof(string) || Nullable.GetUnderlyingType(type) != null || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
	    }

	    private bool IsPrimitiveType(Type parameterType)
	    {
	        return SchemaTypeMapper.Map(parameterType) != null;
	    }

		private IEnumerable<Parameter> GetParameters(string url, IEnumerable<ApiParameterDescription> apiParameterDescriptions)
		{
            var parameters = new List<Parameter>();
			foreach (var apiParam in apiParameterDescriptions)
			{
                parameters.Add(new Parameter(name: apiParam.Name, description: apiParam.Documentation,
                    required: !apiParam.ParameterDescriptor.IsOptional, binding: MapSourceToBinding(apiParam.Source),
                    schema: MapTypeToSchema(apiParam.ParameterDescriptor.ParameterType)));
			}
			return parameters;
		}

        private Shape MapTypeToSchema(Type parameterType)
        {
            return AddType(parameterType); 
        }

        private string MapSourceToBinding(ApiParameterSource source)
        {
            return source.ToString(); //TODO: check
        }

        private static IEnumerable<Parameter> GetParametersFromUrl(string url)
		{
			var dic = new List<Parameter>();

			if (string.IsNullOrWhiteSpace(url) || !url.Contains("{"))
				return dic;

			if (!url.Contains("{"))
				return dic;

			var regex = new Regex("{([^}]+)}");
			var matches = regex.Matches(url);
			foreach (Match match in matches)
			{
                var key = match.Groups[1].Value;

                var parameter = new Parameter (name: null, description: null, required: true, binding: "URL", schema: MapPrimitiveToShape("string"));
				dic.Add(parameter);
			}

			return dic;
		}

        private static Shape MapPrimitiveToShape(string primitiveType)
        {
            return new ScalarShape(primitiveType, null, 0, 0, null, null, null, null, null, 0, null, null, null, null, null, null, null, null, null, null);
        }


        ////TODO: check
        //public class RamlTypesOrderedDictionary
        //{
        //    private readonly OrderedDictionary dic = new OrderedDictionary();
        //    public int Count { get { return dic.Count; } }

        //    public void Clear()
        //    {
        //        dic.Clear();
        //    }

        //    public List<string> Keys { get { return dic.Keys.Cast<string>().ToList(); } }

        //    public IDictionaryEnumerator GetEnumerator()
        //    {
        //        return dic.GetEnumerator();
        //    }

        //    public void Add(string key, Shape value)
        //    {
        //        dic.Add(key, value);
        //    }

        //    public Shape GetByKey(string key)
        //    {
        //        if (!ContainsKey(key))
        //            return null;

        //        var type = dic[key] as Shape;
        //        return type;
        //    }

        //    public bool ContainsKey(string key)
        //    {
        //        return dic.Contains(key);
        //    }

        //    public Shape this[string key]
        //    {
        //        get { return (Shape)dic[key]; }
        //    }
        //}
    }
}
