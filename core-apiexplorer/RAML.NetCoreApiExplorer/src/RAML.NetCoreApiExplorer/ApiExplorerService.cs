using Raml.Parser.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RAML.Api.Core;

namespace RAML.WebApiExplorer
{
	public abstract class ApiExplorerService
	{
		private readonly IApiDescriptionGroupCollectionProvider apiExplorer;
		private readonly string baseUri;
		private SecurityScheme securityScheme;
		private string securityType;

		public IEnumerable<string> SecuredBy { get; set; }

		public IEnumerable<IDictionary<string, SecurityScheme>> SecuritySchemes { get; set; }

		public IEnumerable<Protocol> Protocols { get; set; }

        public string DefaultMediaType { get; set; }

        protected readonly RamlTypesOrderedDictionary RamlTypes = new RamlTypesOrderedDictionary();
        protected readonly IDictionary<string, string> Schemas = new Dictionary<string, string>();
	    protected readonly ICollection<Type> Types = new Collection<Type>();

	    protected ApiExplorerService(IApiDescriptionGroupCollectionProvider apiExplorer, string baseUri = null)
		{
			this.apiExplorer = apiExplorer;
			this.baseUri = baseUri;
		}

	    public abstract RamlDocument GetRaml(string title = null);

	    protected RamlDocument GetRaml(RamlVersion ramlVersion = RamlVersion.Version1, string title = null)
		{
			if (string.IsNullOrWhiteSpace(title))
				title = "Api";

			var raml = new RamlDocument {Title = title, BaseUri = baseUri, RamlVersion = ramlVersion };
			var resourcesDic = new Dictionary<string, Resource>();
			var parameterDescriptionsDic = new Dictionary<string, IList<ApiParameterDescription>>();
		    foreach (var api in apiExplorer.ApiDescriptionGroups.Items)
		    {
		        foreach (var apiDescription in api.Items)
		        {
                    var relativeUri = !apiDescription.ActionDescriptor.AttributeRouteInfo.Template.StartsWith("/")
		                ? "/" + apiDescription.ActionDescriptor.AttributeRouteInfo.Template
		                : apiDescription.ActionDescriptor.AttributeRouteInfo.Template;
		            if (relativeUri.Contains("{controller}"))
		            {
		                relativeUri = relativeUri.Replace("{controller}",
                            apiDescription.ActionDescriptor.RouteValues["controllerName"]);

		                if (relativeUri.EndsWith("/"))
		                    relativeUri = relativeUri.Substring(0, relativeUri.Length - 1);
		            }

		            foreach (var apiParam in GetParametersFromUrl(relativeUri))
		            {
		                relativeUri = RemoveNonExistingParametersFromRoute(relativeUri, apiDescription, apiParam.Key);
		            }

		            Resource resource;
		            if (!resourcesDic.ContainsKey(relativeUri))
		            {
		                resource = new Resource
		                {
		                    Methods = GetMethods(apiDescription, new Collection<string>()),
		                    RelativeUri = relativeUri,
		                };
		                parameterDescriptionsDic.Add(relativeUri, apiDescription.ParameterDescriptions);
		                resourcesDic.Add(relativeUri, resource);
		            }
		            else
		            {
		                resource = resourcesDic[relativeUri];
		                foreach (var apiParameterDescription in apiDescription.ParameterDescriptions)
		                {
		                    parameterDescriptionsDic[relativeUri].Add(apiParameterDescription);
		                }
		                AddMethods(resource, apiDescription, resource.Methods.Select(m => m.Verb).ToList());
		            }

		            if (SetResourceProperties != null)
		                SetResourceProperties(resource, apiDescription);

		            if (SetResourcePropertiesByAction != null)
		                SetResourcePropertiesByAction(resource, apiDescription.ActionDescriptor);

		            //if (SetResourcePropertiesByController != null)
		            //    SetResourcePropertiesByController(resource, apiDescription.ActionDescriptor.ControllerDescriptor);
		        }
		    }

		    raml.Schemas = new List<IDictionary<string, string>> { Schemas };

		    raml.Types = RamlTypes;

			OrganizeResourcesHierarchically(raml, resourcesDic);

			SetUriParameters(raml.Resources, parameterDescriptionsDic, string.Empty);

			if(SetRamlProperties != null)
				SetRamlProperties(raml);

			if (SecuritySchemes != null)
				raml.SecuritySchemes = SecuritySchemes;
			
			if (!string.IsNullOrWhiteSpace(securityType) && securityScheme != null)
				SetSecurityScheme(raml);

			if (SecuredBy != null)
				raml.SecuredBy = SecuredBy;

			if(Protocols != null)
				raml.Protocols = Protocols;

			return raml;
		}

		public void SetSecurityScheme(string type, SecurityScheme scheme)
		{
			securityScheme = scheme;
			securityType = type;
		}

		public void UseOAuth2(string authorizationUri, string accessTokenUri, IEnumerable<string> authorizationGrants, IEnumerable<string> scopes, SecuritySchemeDescriptor securitySchemeDescriptor)
		{
			securityType = "oauth_2_0";
			var securitySettings = new SecuritySettings
			                       {
				                       AuthorizationUri = authorizationUri,
				                       AccessTokenUri = accessTokenUri,
				                       AuthorizationGrants = authorizationGrants,
				                       Scopes = scopes
			                       };
			securityScheme = new SecurityScheme
			                 {
				                 DescribedBy = securitySchemeDescriptor,
				                 Settings = securitySettings,
				                 Type = new Dictionary<string, IDictionary<string, string>> {{"OAuth 2.0", null}}
			                 };
		}

		public void UseOAuth1(string authorizationUri, string requestTokenUri, string tokenCredentialsUri, SecuritySchemeDescriptor securitySchemeDescriptor)
		{
			securityType = "oauth_1_0";
			var securitySettings = new SecuritySettings
			                       {
				                       AuthorizationUri = authorizationUri,
				                       RequestTokenUri = requestTokenUri,
				                       TokenCredentialsUri = tokenCredentialsUri
			                       };
			securityScheme = new SecurityScheme
			                 {
				                 DescribedBy = securitySchemeDescriptor,
				                 Settings = securitySettings,
				                 Type = new Dictionary<string, IDictionary<string, string>> {{"OAuth 1.0", null}}
			                 };
		}

		public Action<RamlDocument> SetRamlProperties { get; set; }

		public Action<Resource, ApiDescription> SetResourceProperties  { get; set; }

		//public Action<Resource, HttpControllerDescriptor> SetResourcePropertiesByController { get; set; }

		public Action<Resource, ActionDescriptor> SetResourcePropertiesByAction { get; set; }

        public Action<ApiDescription, Method> SetMethodProperties { get; set; }

        protected abstract string AddType(Type type);
        protected abstract MimeType CreateMimeType(string type);
        protected abstract string MapParam(Type type);
        protected abstract string Map(Type type);
        protected abstract void GetQueryParameter(ApiParameterDescription apiParam, Dictionary<string, Parameter> queryParams);

        private void SetSecurityScheme(RamlDocument raml)
		{
			var securitySchemes = new List<IDictionary<string, SecurityScheme>>();

			if (raml.SecuritySchemes != null && raml.SecuritySchemes.Any())
				securitySchemes = raml.SecuritySchemes.ToList();

			var schemes = new Dictionary<string, SecurityScheme> { { securityType, securityScheme } };
			securitySchemes.Add(schemes);

			raml.SecuritySchemes = securitySchemes;
		}

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

		private void SetUriParameters(IEnumerable<Resource> resources, Dictionary<string, IList<ApiParameterDescription>> parameterDescriptionsDic, string parentUrl)
		{
			if(resources == null)
				return;

			foreach (var resource in resources)
			{
				var fullUrl = parentUrl + resource.RelativeUri;
				resource.UriParameters = GetUriParameters(resource.RelativeUri, parameterDescriptionsDic[fullUrl]);
				SetUriParameters(resource.Resources, parameterDescriptionsDic, fullUrl);
			}
		}

		private void OrganizeResourcesHierarchically(RamlDocument raml, Dictionary<string, Resource> resourcesDic)
		{
			foreach (var kv in resourcesDic)
			{
				var matchingResources = resourcesDic.Where(r => r.Key != kv.Key && kv.Key.StartsWith(r.Key + "/"));
				if (matchingResources.Any())
				{
					var parent = matchingResources.OrderByDescending(r => r.Key.Length).First();
					kv.Value.RelativeUri = kv.Value.RelativeUri.Substring(parent.Key.Length); // remove parent route from relative uri
					parent.Value.Resources.Add(kv.Value);
				}
				else
				{
					raml.Resources.Add(kv.Value);
				}
			}
		}

		private void AddMethods(Resource resource, ApiDescription api, ICollection<string> verbs)
		{
			var methods = resource.Methods.ToList();
			var newMethods = GetMethods(api, verbs);
			methods.AddRange(newMethods);
			resource.Methods = methods;
		}

	    protected string GetDescription(ApiDescription api)
		{
			var description = string.Empty;
			
			if (!string.IsNullOrWhiteSpace(api.ActionDescriptor.DisplayName))
				description += api.ActionDescriptor.DisplayName;

			//description += " (" + api.ActionDescriptor.ControllerDescriptor.ControllerName + "." + api.ActionDescriptor.ActionName + ")";

			return description;
		}

	    protected abstract IEnumerable<Method> GetMethods(ApiDescription api, ICollection<string> verbs);

	    protected IEnumerable<Response> GetResponses(IList<ApiResponseType> responseTypes, ApiDescription api)
		{
            var responses = new List<Response>();

		    foreach (var apiResponseType in responseTypes)
		    {
		        if (apiResponseType?.Type == null)
		            return responses;

		        var responseType = apiResponseType.Type;

		        //var attributes = api.ActionDescriptor.AttributeRouteInfo;
		        //responses = HandleResponseTypeStatusAttributes(attributes);

		        //if (responseType == typeof (IHttpResult))
		        //    return responses;

		        responses.Add(HandleResponseTypeAttributes(apiResponseType));
		    }

		    return responses;
		}

	    private Response HandleResponseTypeAttributes(ApiResponseType responseType)
	    {
            var type = AddType(responseType.Type);

            return new Response
            {
                Body = CreateJsonMimeType(type),
                Code = responseType.StatusCode
            };
	    }

	    //private List<Response> HandleResponseTypeStatusAttributes(IEnumerable<Attribute> attributes)
	    //{
     //       var responses = new Dictionary<string,Response>();
     //       foreach (var attribute in attributes.Where(a => a is ResponseTypeStatusAttribute))
     //       {
     //           var response = GetResponse(attribute);
     //           if(!responses.ContainsKey(response.Code))
     //               responses.Add(response.Code, response);
     //       }
	    //    return responses.Values.ToList();
	    //}

	    private Response GetResponse(Attribute attribute)
	    {
            var status = ((ResponseTypeStatusAttribute)attribute).StatusCode;
            var type = ((ResponseTypeStatusAttribute)attribute).ResponseType;
            var typeName = AddType(type);
            return new Response
            {
                Code = (int)status,
                Body = CreateJsonMimeType(typeName)
            };
	    }

        protected Dictionary<string, MimeType> CreateJsonMimeType(string type)
        {
            var mimeType = CreateMimeType(type);
            return CreateMimeTypes(mimeType);
        }

	    protected Dictionary<string, MimeType> CreateMimeTypes(MimeType mimeType)
	    {
	        var mimeTypes = new Dictionary<string, MimeType>
	        {
	            {
	                "application/json",
	                mimeType
	            }
	        };
	        return mimeTypes;
	    }

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

	    protected Dictionary<string, MimeType> GetRequestMimeTypes(ApiDescription api)
		{
			var mediaTypes = api.SupportedRequestFormats.Select(f => f.MediaType).ToArray();
			var mimeTypes = new Dictionary<string, MimeType>();

			var apiParam = api.ParameterDescriptions.FirstOrDefault(p => p.Source == BindingSource.Body);
			MimeType mimeType = null;

			if (apiParam != null)
			{
				var type = apiParam.Type;

                var typeName = AddType(type);

				mimeType = CreateMimeType(typeName);
			}

			if(mimeType != null && !mediaTypes.Any())
				mimeTypes.Add("application/json", mimeType);

			foreach (var mediaType in mediaTypes)
			{
                if(!mimeTypes.ContainsKey(mediaType))
				    mimeTypes.Add(mediaType, mediaType == "application/json" ? mimeType : new MimeType());
			}
			
			return mimeTypes;
		}

	    protected IDictionary<string, Parameter> GetQueryParameters(IEnumerable<ApiParameterDescription> parameterDescriptions)
        {
            var queryParams = new Dictionary<string, Parameter>();

            foreach (var apiParam in parameterDescriptions.Where(p => p.Source == BindingSource.Query || p.Source == BindingSource.ModelBinding))
            {
                GetQueryParameter(apiParam, queryParams);
            }
            return queryParams;
        }

	    protected Parameter GetPrimitiveParameter(ControllerParameterDescriptor apiParam)
        {
            var netType = apiParam.ParameterType;
            var ramlType = MapParam(netType);

            return GetParameter(apiParam, ramlType, netType);
        }

	    protected Parameter GetParameter(ControllerParameterDescriptor apiParam, string ramlType, Type parameterType)
        {
            var parameter = new Parameter
            {
                Default = apiParam.ParameterInfo.HasDefaultValue
                    ? null
                    : apiParam.ParameterInfo.DefaultValue.ToString(),
                Required = !apiParam.ParameterInfo.IsOptional,
                Type = ramlType,
                DisplayName = apiParam.Name
            };

            return parameter;
        }

        protected Parameter GetPrimitiveParameter(ApiParameterDescription apiParam)
	    {
            var netType = apiParam.Type;
            var ramlType = MapParam(netType);

            return GetParameter(apiParam, ramlType, netType);
	    }

        protected Parameter GetParameter(ApiParameterDescription apiParam, string ramlType, Type netType)
	    {
	        var parameter = new Parameter
	        {
	            Default = apiParam.ModelMetadata != null && apiParam.ModelMetadata.IsReferenceOrNullableType
	                      && !apiParam.ModelMetadata.IsRequired
	                ? "null"
	                : null,
	            Required = apiParam.ModelMetadata?.IsRequired ?? false,
	            Type = ramlType,
                DisplayName = apiParam.Name,
                Description = apiParam.ModelMetadata?.Description
	        };

	        return parameter;
	    }

	    protected static bool IsNullable(Type type)
	    {
	        return type == typeof (string) || Nullable.GetUnderlyingType(type) != null ||
	               (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>));
	    }

	    protected bool IsPrimitiveType(Type parameterType)
	    {
	        return Map(parameterType) != null;
	    }

		private IDictionary<string, Parameter> GetUriParameters(string url, IEnumerable<ApiParameterDescription> apiParameterDescriptions)
		{
			var urlParameters = GetParametersFromUrl(url);
			var parameterDescriptions = apiParameterDescriptions
				.Where(apiParam => urlParameters.ContainsKey(apiParam.Name) 
					&& !string.IsNullOrWhiteSpace(apiParam.ModelMetadata?.Description));

			foreach (var apiParam in parameterDescriptions)
			{
				urlParameters[apiParam.Name].Description = apiParam.ModelMetadata?.Description;
			}
			return urlParameters;
		}

		private static IDictionary<string, Parameter> GetParametersFromUrl(string url)
		{
			var dic = new Dictionary<string, Parameter>();

			if (string.IsNullOrWhiteSpace(url) || !url.Contains("{"))
				return dic;

			if (!url.Contains("{"))
				return dic;

			var regex = new Regex("{([^}]+)}");
			var matches = regex.Matches(url);
			foreach (Match match in matches)
			{
				var parameter = new Parameter {Required = true, Type = "string"};
				dic.Add(match.Groups[1].Value, parameter);
			}

			return dic;
		}
	}
}
