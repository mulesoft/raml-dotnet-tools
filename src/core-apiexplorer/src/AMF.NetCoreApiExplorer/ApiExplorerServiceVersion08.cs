using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Raml.Parser.Expressions;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace AMF.WebApiExplorer
{
    public class ApiExplorerServiceVersion08 : ApiExplorerService
    {
        private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

        public ApiExplorerServiceVersion08(IApiDescriptionGroupCollectionProvider apiExplorer, string baseUri = null) : base(apiExplorer, baseUri)
        {
        }

        public override RamlDocument GetRaml(string title = null)
        {
            return GetRaml(RamlVersion.Version08, title);
        }

        protected override string AddType(Type type)
        {
            var schemaName = type.Name.Replace("`", string.Empty);
            if (Types.Contains(type))
                return schemaName;

            var schema = schemaBuilder.Get(type);

            if (string.IsNullOrWhiteSpace(schema))
                return string.Empty;

            // handle case of different types with same class name
            if (Schemas.ContainsKey(schemaName))
                schemaName = GetUniqueSchemaName(schemaName);

            Schemas.Add(schemaName, schema);
            Types.Add(type);

            return schemaName;
        }

        protected override MimeType CreateMimeType(string type)
        {
            return new MimeType
            {
                Schema = type
            };
        }

        protected override void GetQueryParameter(ApiParameterDescription apiParam, Dictionary<string, Parameter> queryParams)
        {
            if (!IsPrimitiveType(apiParam.Type))
            {
                GetParametersFromComplexType(apiParam, queryParams);
            }
            else
            {
                var parameter = GetPrimitiveParameter(apiParam);

                if (!queryParams.ContainsKey(apiParam.Name))
                    queryParams.Add(apiParam.Name, parameter);
            }
        }

        private void GetParametersFromComplexType(ApiParameterDescription apiParam, IDictionary<string, Parameter> queryParams)
        {
            var properties = apiParam.Type.GetProperties().Where(p => p.CanWrite);
            foreach (var property in properties)
            {
                if (!IsPrimitiveType(property.PropertyType))
                    continue;

                var parameter = GetParameterFromProperty(apiParam, property);

                if (!queryParams.ContainsKey(property.Name))
                    queryParams.Add(property.Name, parameter);
            }
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
                QueryParameters = GetQueryParameters(api.ParameterDescriptions),
                Body = GetRequestMimeTypes(api),
                Responses = GetResponses(api.SupportedResponseTypes, api),
            };
            methods.Add(method);
            verbs.Add(verb);

            SetMethodProperties?.Invoke(api, method);


            return methods;
        }

        private Parameter GetParameterFromProperty(ApiParameterDescription apiParam, PropertyInfo property)
        {
            var parameter = new Parameter
            {
                Default = IsNullable(property.PropertyType) ? "null" : null,
                Required = !IsNullable(property.PropertyType),
                Type = MapParam(property.PropertyType),
                DisplayName = apiParam.Name,
                Description = apiParam.ModelMetadata?.Description
            };
            return parameter;
        }

        protected override string MapParam(Type type)
        {
            return SchemaTypeMapper.Map(type);
        }

        protected override string Map(Type type)
        {
            if (type == typeof(DateTime))
                return "date";

            return SchemaTypeMapper.Map(type);
        }
    }
}