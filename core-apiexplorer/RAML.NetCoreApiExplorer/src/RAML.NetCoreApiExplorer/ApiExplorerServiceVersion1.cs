using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Raml.Parser.Expressions;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace RAML.WebApiExplorer
{
    public class ApiExplorerServiceVersion1 : ApiExplorerService
    {
        private readonly Raml1TypeBuilder typeBuilder;

        public ApiExplorerServiceVersion1(IApiDescriptionGroupCollectionProvider apiExplorer, string baseUri = null)
            : base(apiExplorer, baseUri)
        {
            typeBuilder = new Raml1TypeBuilder(RamlTypes);
        }

        public override RamlDocument GetRaml(string title = null)
        {
            return GetRaml(RamlVersion.Version1, title);
        }

        protected override string AddType(Type type)
        {
            var typeName = typeBuilder.Add(type);
            return typeName;
        }

        protected override void GetQueryParameter(ApiParameterDescription apiParam, Dictionary<string, Parameter> queryParams)
        {
            Parameter parameter;
            if (!IsPrimitiveType(apiParam.Type))
            {
                var typeName = typeBuilder.Add(apiParam.Type);
                parameter = GetParameter(apiParam, typeName, apiParam.Type);
            }
            else
            {
                parameter = GetPrimitiveParameter(apiParam);
            }

            if (!queryParams.ContainsKey(apiParam.Name))
                queryParams.Add(apiParam.Name, parameter);
        }

        private void GetQueryParameter(ControllerParameterDescriptor apiParam, Dictionary<string, Parameter> queryParams)
        {
            Parameter parameter;
            if (!IsPrimitiveType(apiParam.ParameterType))
            {
                var typeName = typeBuilder.Add(apiParam.ParameterType);
                parameter = GetParameter(apiParam, typeName, apiParam.ParameterType);
            }
            else
            {
                parameter = GetPrimitiveParameter(apiParam);
            }

            if (!queryParams.ContainsKey(apiParam.Name))
                queryParams.Add(apiParam.Name, parameter);
        }

        protected override IEnumerable<Method> GetMethods(ApiDescription api, ICollection<string> verbs)
        {
            var methods = new Collection<Method>();


            var verb = api.HttpMethod.ToLowerInvariant();
            if (verbs.Contains(verb))
                return methods;

            var method = new Method
            {
                Description = GetDescription(api),
                Verb = verb,
                QueryParameters = GetQueryParameters(api.ActionDescriptor.Parameters), // GetQueryParameters(api.ParameterDescriptions),
                Body = GetRequestMimeTypes(api),
                Responses = GetResponses(api.SupportedResponseTypes, api),
            };
            methods.Add(method);
            verbs.Add(verb);

            SetMethodProperties?.Invoke(api, method);


            return methods;
        }

        protected IDictionary<string, Parameter> GetQueryParameters(IList<ParameterDescriptor> parameterDescriptions)
        {
            var queryParams = new Dictionary<string, Parameter>();

            //var paramsDescrips = parameterDescriptions.Select(p => (ControllerParameterDescriptor) p).ToList();

            foreach (var parameterDescriptor in parameterDescriptions.Where(
                        p => p.BindingInfo == null || p.BindingInfo.BindingSource == BindingSource.Query ||
                            p.BindingInfo.BindingSource == BindingSource.ModelBinding))
            {
                var apiParam = parameterDescriptor as ControllerParameterDescriptor;
                GetQueryParameter(apiParam, queryParams);
            }

            return queryParams;
        }


        protected override MimeType CreateMimeType(string type)
        {
            return new MimeType
            {
                Type = type
            };
        }

        protected override string MapParam(Type type)
        {
            return Map(type);
        }

        protected override string Map(Type type)
        {
            return Raml1TypeMapper.Map(type);
        }
    }
}