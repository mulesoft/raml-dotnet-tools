using System.Collections.Generic;
using Newtonsoft.JsonV4.Schema;

namespace Raml.Tools.JSON
{
    public class JsonSchemaCustomV4Resolver : JsonSchemaResolver
    {
        private readonly IDictionary<string, ApiObject> objects;

        public JsonSchemaCustomV4Resolver(IDictionary<string, ApiObject> objects)
        {
            this.objects = objects;
        }

        public override JsonSchema GetSchema(string reference)
        {
            var schema = base.GetSchema(reference);

            if (schema != null) return schema;

            if (!objects.ContainsKey(reference))
                return null;

            var jsonSchema = objects[reference].JSONSchema.Replace("\\\"", "\"");
            return JsonSchema.Parse(jsonSchema, new JsonSchemaCustomV4Resolver(objects));
        }
    }
}