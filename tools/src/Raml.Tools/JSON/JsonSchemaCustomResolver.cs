using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace Raml.Tools.JSON
{
    public class JsonSchemaCustomResolver : JsonSchemaResolver
    {
        private readonly IDictionary<string, ApiObject> objects;
        private readonly IDictionary<string, ApiObject> schemaObjects;

        public JsonSchemaCustomResolver(IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> schemaObjects)
        {
            this.objects = objects;
            this.schemaObjects = schemaObjects;
        }

        public override JsonSchema GetSchema(string reference)
        {
            var schema = base.GetSchema(reference);

            if (schema != null) return schema;

            if (!objects.ContainsKey(reference) && !schemaObjects.ContainsKey(reference))
                return null;

            string jsonSchema;

            if (objects.ContainsKey(reference))
            {
                jsonSchema = objects[reference].JSONSchema.Replace("\\\"", "\"");
            }
            else
            {
                jsonSchema = schemaObjects[reference].JSONSchema.Replace("\\\"", "\"");
            }
            return JsonSchema.Parse(jsonSchema, new JsonSchemaCustomResolver(objects, schemaObjects));
        }
    }
}