using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class ApiExplorerServiceVersion08 : ApiExplorerService
    {
        private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

        public ApiExplorerServiceVersion08(IApiExplorer apiExplorer, string baseUri = null) : base(apiExplorer, baseUri)
        {
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

    }
}