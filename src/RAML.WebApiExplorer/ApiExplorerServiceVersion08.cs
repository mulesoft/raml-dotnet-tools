using System;
using System.Web.Http.Description;

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
            if (SchemasOrTypes.ContainsKey(schemaName))
                schemaName = GetUniqueSchemaName(schemaName);

            SchemasOrTypes.Add(schemaName, schema);
            Types.Add(type);

            return schemaName;
        }
    }
}