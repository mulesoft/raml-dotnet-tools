using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using AMF.Parser.Model;

namespace RAML.WebApiExplorer
{
    public class ApiExplorerServiceVersion08 : ApiExplorerService
    {
        private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

        public ApiExplorerServiceVersion08(IApiExplorer apiExplorer, string baseUri = null) : base(apiExplorer, baseUri)
        {
        }

        protected override Shape AddType(Type type)
        {
            var schemaName = type.Name.Replace("`", string.Empty);
            if (Types.ContainsKey(type))
                return Types[type];

            var schema = schemaBuilder.Get(type);

            if (string.IsNullOrWhiteSpace(schema))
                return null;

            // handle case of different types with same class name
            if (Schemas.ContainsKey(schemaName))
                schemaName = GetUniqueSchemaName(schemaName);

            Schemas.Add(schemaName, schema);
            var shape = new SchemaShape("application/json", schema, null, null, null, type.FullName, null, null, null, null, null, null);
            Types.Add(type, shape);

            return shape;
        }

        protected override Shape CreateMimeType(string type)
        {
            return new TypeToShapeConverter().Convert(type);
        }

    }
}