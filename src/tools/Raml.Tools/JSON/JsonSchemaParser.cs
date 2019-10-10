using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Schema;
using Raml.Common;

namespace Raml.Tools.JSON
{
    public class JsonSchemaParser
    {
        private readonly ICollection<string> ids  = new Collection<string>();
        private IDictionary<string, ApiObject> otherObjects = new Dictionary<string, ApiObject>();
        private IDictionary<string, ApiObject> schemaObjects = new Dictionary<string, ApiObject>();
        private string className;

        public ApiObject Parse(string key, string jsonSchema, IDictionary<string, ApiObject> objects, IDictionary<string, string> warnings, 
            IDictionary<string, ApiEnum> enums, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            this.otherObjects = otherObjects;
            this.schemaObjects = schemaObjects;
            className = NetNamingMapper.GetObjectName(key);
            var obj = new ApiObject
                      {
                          Name = className,
                          Properties = new List<Property>(),
                          JSONSchema = jsonSchema.Replace(Environment.NewLine, "").Replace("\r\n", "").Replace("\n", "")
                                                 .Replace("\\", "\\\\").Replace("\"", "\\\"")
                                                // .Replace("\\/", "\\\\/").Replace("\"", "\\\"").Replace("\\\\\"", "\\\\\\\"")
                      };
            JsonSchema schema = null;
            Newtonsoft.JsonV4.Schema.JsonSchema v4Schema = null;
            if (jsonSchema.Contains("\"oneOf\""))
            {
                v4Schema = ParseV4Schema(key, jsonSchema, warnings, objects);
            }
            else
            {
                schema = ParseV3OrV4Schema(key, jsonSchema, warnings, ref v4Schema, objects, schemaObjects);
            }

            if (schema == null && v4Schema == null)
                return obj;

            if (schema != null)
            {
                if (schema.Type == JsonSchemaType.Array)
                {
                    obj.IsArray = true;
                    if (schema.Items != null && schema.Items.Any())
                    {
                        if (schema.Items.First().Properties != null)
                        {
                            ParseProperties(objects, obj.Properties, schema.Items.First().Properties, enums);
                        }
                        else
                        {
                            obj.Type = NetTypeMapper.GetNetType(schema.Items.First().Type, schema.Items.First().Format);
                        }
                    }
                }
                else
                {
                    ParseProperties(objects, obj.Properties, schema.Properties, enums);
                    AdditionalProperties(obj.Properties, schema);
                }
            }
            else
            {
                if (v4Schema.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.Array)
                {
                    obj.IsArray = true;
                    if (v4Schema.Items != null && v4Schema.Items.Any())
                    {
                        if (v4Schema.Items.First().Properties != null)
                        {
                            ParseProperties(objects, obj.Properties, v4Schema.Items.First(), enums);
                        }
                        else
                        {
                            obj.Type = NetTypeMapper.GetNetType(v4Schema.Items.First().Type, v4Schema.Items.First().Format);
                        }
                    }
                        
                }
                else
                {
                    ParseProperties(objects, obj.Properties, v4Schema, enums);
                }
            }
            return obj;
        }

        private static JsonSchema ParseV3OrV4Schema(string key, string jsonSchema, IDictionary<string, string> warnings, 
            ref Newtonsoft.JsonV4.Schema.JsonSchema v4Schema, IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> schemaObjects)
        {
            JsonSchema schema = null;
            try
            {
                schema = JsonSchema.Parse(jsonSchema, new JsonSchemaCustomResolver(objects, schemaObjects));
            }
            catch (Exception exv3) // NewtonJson does not support Json Schema v4
            {
                try
                {
                    schema = null;
                    v4Schema = Newtonsoft.JsonV4.Schema.JsonSchema.Parse(jsonSchema, new JsonSchemaCustomV4Resolver(objects));
                }
                catch (Exception exv4)
                {
                    if (!warnings.ContainsKey(key))
                        warnings.Add(key,
                            "Could not parse JSON Schema. v3 parser message: " +
                            exv3.Message.Replace("\r\n", string.Empty).Replace("\n", string.Empty) +
                            ". v4 parser message: " +
                            exv4.Message.Replace("\r\n", string.Empty).Replace("\n", string.Empty));
                }
            }
            return schema;
        }

        private static Newtonsoft.JsonV4.Schema.JsonSchema ParseV4Schema(string key, string jsonSchema, IDictionary<string, string> warnings, IDictionary<string, ApiObject> objects)
        {
            Newtonsoft.JsonV4.Schema.JsonSchema v4Schema = null;
            try
            {
                v4Schema = Newtonsoft.JsonV4.Schema.JsonSchema.Parse(jsonSchema, new JsonSchemaCustomV4Resolver(objects));
            }
            catch (Exception exv4)
            {
                if (!warnings.ContainsKey(key))
                    warnings.Add(key,
                        "Could not parse JSON Schema. " +
                        exv4.Message.Replace("\r\n", string.Empty).Replace("\n", string.Empty));
            }
            return v4Schema;
        }

        private string ParseObject(string key, JsonSchema schema, IDictionary<string, ApiObject> objects, IDictionary<string, ApiEnum> enums)
        {
            var propertiesSchemas = schema.Properties;

            var obj = new ApiObject
            {
                Name = NetNamingMapper.GetObjectName(key),
                Properties = ParseSchema(propertiesSchemas, objects, enums)
            };

            AdditionalProperties(obj.Properties, schema);

            if(!obj.Properties.Any())
                return null;

            // Avoid duplicated keys and names or no properties
            if (objects.ContainsKey(key) || objects.Any(o => o.Value.Name == obj.Name) 
                || otherObjects.ContainsKey(key) || otherObjects.Any(o => o.Value.Name == obj.Name)
                || schemaObjects.ContainsKey(key) || schemaObjects.Any(o => o.Value.Name == obj.Name))
            {
                if (UniquenessHelper.HasSameProperties(obj, objects, key, otherObjects, schemaObjects))
                    return key;

                obj.Name = UniquenessHelper.GetUniqueName(objects, obj.Name, otherObjects, schemaObjects);
                key = UniquenessHelper.GetUniqueKey(objects, key, otherObjects);
            }

            objects.Add(key, obj);
            return key;
        }


        private IList<Property> ParseSchema(IDictionary<string, JsonSchema> schema, IDictionary<string, ApiObject> objects, IDictionary<string, ApiEnum> enums)
        {
            var props = new List<Property>();
            
            if (schema == null)
                return props;

            foreach (var kv in schema)
            {
                var isEnum = kv.Value.Enum != null && kv.Value.Enum.Any();

                var enumName = string.Empty;
                if (isEnum)
                {
                    enumName = ParseEnum(kv.Key, kv.Value, enums, kv.Value.Description);
                }

                var type = GetType(kv, isEnum, enumName);
                if(type == null)
                    continue;

                var prop = CreateProperty(kv.Key, type, kv, isEnum);

                ParseComplexTypes(objects, kv.Value, prop, kv, kv.Key, enums);
                props.Add(prop);
            }
            return props;
        }


        private void ParseProperties(IDictionary<string, ApiObject> objects, ICollection<Property> props, Newtonsoft.JsonV4.Schema.JsonSchema schema, IDictionary<string, ApiEnum> enums)
        {
            var properties = schema.Properties;

            if (properties == null)
                return;

            foreach (var property in properties)
            {
                if ((property.Value.Enum != null && !property.Value.Enum.Any()) && (property.Value.Type == null || property.Value.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.Null ||
                    property.Value.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.None))
                    continue;

                var isEnum = property.Value.Enum != null && property.Value.Enum.Any();

                var enumName = string.Empty;
                if (isEnum)
                {
                    enumName = ParseEnum(property.Key, property.Value, enums, property.Value.Description);
                }

                var prop = CreateProperty(schema, property, isEnum, enumName);

                ParseComplexTypes(objects, schema, property.Value, prop, property, enums);
                props.Add(prop);
            }

            AdditionalProperties(props, schema);
        }

        private Property CreateProperty(string key, string type, KeyValuePair<string, JsonSchema> property, bool isEnum)
        {
            return new Property(className)
            {
                Name = NetNamingMapper.GetPropertyName(key),
                OriginalName = key,
                Type = type,
                Description = property.Value.Description,
                IsEnum = isEnum,
                MaxLength = property.Value.MaximumLength,
                MinLength = property.Value.MinimumLength,
                Maximum = property.Value.Maximum,
                Minimum = property.Value.Minimum,
                Required = property.Value.Required != null && property.Value.Required.Value
            };
        }

        private Property CreateProperty(Newtonsoft.JsonV4.Schema.JsonSchema schema, KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property, bool isEnum, string enumName)
        {
            return new Property(className)
            {
                Name = NetNamingMapper.GetPropertyName(property.Key),
                Type = GetType(property, isEnum, enumName, schema.Required),
                OriginalName = property.Key,
                Description = property.Value.Description,
                IsEnum = isEnum,
                Required = schema.Required != null && schema.Required.Contains(property.Key),
                MaxLength = property.Value.MaximumLength,
                MinLength = property.Value.MinimumLength,
                Maximum = property.Value.Maximum,
                Minimum = property.Value.Minimum
            };
        }

        private static void AdditionalProperties(ICollection<Property> props, JsonSchema schema)
        {
            if (schema.AdditionalProperties == null || !schema.AdditionalProperties.AllowAdditionalProperties) return;

            AddAdditionalPropertiesProperty(props);
        }

        private static void AdditionalProperties(ICollection<Property> props, Newtonsoft.JsonV4.Schema.JsonSchema schema)
        {
            if (schema.AdditionalProperties == null || !schema.AdditionalProperties.AllowAdditionalProperties) return;

            AddAdditionalPropertiesProperty(props);
        }

        private static void AddAdditionalPropertiesProperty(ICollection<Property> props)
        {
            props.Add(new Property
            {
                Name = "AdditionalProperties",
                Type = "IDictionary<string, object>"
            });
        }

        private static string GetType(KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property, bool isEnum, string enumName, ICollection<string> requiredProps)
        {
            if (property.Value.OneOf != null && property.Value.OneOf.Count > 0)
                return NetNamingMapper.GetObjectName(property.Key);

            if (isEnum) 
                return enumName;

            var type = NetTypeMapper.GetNetType(property.Value.Type, property.Value.Format);

            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type == "string" || (requiredProps != null && requiredProps.Contains(property.Key)))
                    return type;

                return type + "?";
            }

            if (HasMultipleTypes(property))
                return HandleMultipleTypes(property);

            if (!string.IsNullOrWhiteSpace(property.Value.Id))
                return NetNamingMapper.GetObjectName(property.Value.Id);

            return NetNamingMapper.GetObjectName(property.Key);
        }

        private static string HandleMultipleTypes(KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property)
        {
            var type = "object";
            var types = property.Value.Type.ToString().Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            if (types.Length == 2)
            {
                type = types[0] == "Null"
                    ? NetTypeMapper.GetNetType(types[1].ToLowerInvariant(), property.Value.Format)
                    : NetTypeMapper.GetNetType(types[0].ToLowerInvariant(), property.Value.Format);
                type = IsNullableType(type) ? type + "?" : type;
            }
            return type;
        }

        private static bool IsNullableType(string type)
        {
            return type != "string";
        }

        private static bool HasMultipleTypes(KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property)
        {
            return property.Value.Type != null && property.Value.Type.ToString().Contains(",") && property.Value.Type.ToString().Contains("Null") && !property.Value.Type.ToString().Contains("Object");
        }

        private static string GetType(KeyValuePair<string, JsonSchema> property, bool isEnum, string enumName)
        {
            if (isEnum)
                return enumName;

            var type = NetTypeMapper.GetNetType(property.Value.Type, property.Value.Format);

            if (!string.IsNullOrWhiteSpace(type))
            {
                if(type == "string" || (property.Value.Required != null && property.Value.Required.Value))
                    return type;

                return type + "?";
            }

            if (HasMultipleTypes(property))
                return HandleMultipleTypes(property);

            if (!string.IsNullOrWhiteSpace(property.Value.Id))
                return NetNamingMapper.GetObjectName(property.Value.Id);

            // if it is a "body less" array then I assume it's an array of strings
            if (property.Value.Type == JsonSchemaType.Array && (property.Value.Items == null || !property.Value.Items.Any()))
                return CollectionTypeHelper.GetCollectionType("string");

            // if it is a "body less" object then use object as the type
            if (property.Value.Type == JsonSchemaType.Object && (property.Value.Properties == null || !property.Value.Properties.Any()))
                return "object";

            if(property.Value == null)
                return null;

            return NetNamingMapper.GetObjectName(property.Key);
        }

        private static string HandleMultipleTypes(KeyValuePair<string, JsonSchema> property)
        {
            var type = "object";
            var types = property.Value.Type.ToString().Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            if (types.Length == 2)
            {
                type = types[0] == "Null"
                    ? NetTypeMapper.GetNetType(types[1].ToLowerInvariant(), property.Value.Format)
                    : NetTypeMapper.GetNetType(types[0].ToLowerInvariant(), property.Value.Format);
                type = IsNullableType(type) ? type + "?" : type;
            }
            return type;
        }

        private static bool HasMultipleTypes(KeyValuePair<string, JsonSchema> property)
        {
            return property.Value.Type != null && property.Value.Type.ToString().Contains(",") && property.Value.Type.ToString().Contains("Null") && !property.Value.Type.ToString().Contains("Object");
        }

        private void ParseComplexTypes(IDictionary<string, ApiObject> objects, Newtonsoft.JsonV4.Schema.JsonSchema schema, Newtonsoft.JsonV4.Schema.JsonSchema propertySchema, Property prop, KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property, IDictionary<string, ApiEnum> enums)
        {
            if (propertySchema.Type.HasValue
                && (propertySchema.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.Object || propertySchema.Type.Value.ToString().Contains("Object")) 
                && (propertySchema.OneOf == null || propertySchema.OneOf.Count == 0 || (schema != null && (schema.Definitions == null || schema.Definitions.Count == 0))))
            {
                if (!string.IsNullOrWhiteSpace(schema?.Id) && ids.Contains(schema.Id))
                    return;

                if (!string.IsNullOrWhiteSpace(schema?.Id))
                    ids.Add(schema.Id);

                var type = string.IsNullOrWhiteSpace(property.Value.Id) ? property.Key : property.Value.Id;
                type = ParseObject(type, propertySchema, objects, enums, propertySchema);
                prop.Type = NetNamingMapper.GetObjectName(type);
            }
            else if (propertySchema.Type.HasValue && schema != null
                && propertySchema.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.Object && propertySchema.OneOf != null && propertySchema.OneOf.Count > 0 && schema.Definitions != null && schema.Definitions.Count > 0)
            {
                string baseTypeName = NetNamingMapper.GetObjectName(property.Key);

                if (schemaObjects.ContainsKey(baseTypeName) || objects.ContainsKey(baseTypeName) || otherObjects.ContainsKey(baseTypeName))
                    return;

                objects.Add(baseTypeName,
                    new ApiObject
                    {
                        Name = baseTypeName,
                        Properties = new List<Property>()
                    });

                foreach(var innerSchema in propertySchema.OneOf)
                {
                    var definition = schema.Definitions.FirstOrDefault(k => k.Value == innerSchema);
                    ParseObject(property.Key + definition.Key, innerSchema, objects, enums, innerSchema, baseTypeName);
                                       
                }

                prop.Type = baseTypeName;
            }
            else if (propertySchema.Type == Newtonsoft.JsonV4.Schema.JsonSchemaType.Array)
            {
                ParseArray(objects, propertySchema, prop, property, enums);
            }
            
        }

        private string ParseObject(string key, Newtonsoft.JsonV4.Schema.JsonSchema schema, IDictionary<string, ApiObject> objects, 
            IDictionary<string, ApiEnum> enums, Newtonsoft.JsonV4.Schema.JsonSchema parentSchema, string baseClass = null)
        {
            var propertiesSchemas = schema.Properties;
            var obj = new ApiObject
                      {
                          Name = NetNamingMapper.GetObjectName(key),
                          Properties = ParseSchema(propertiesSchemas, objects, enums, parentSchema),
                          BaseClass = baseClass
                      };

            AdditionalProperties(obj.Properties, schema);

            if (!obj.Properties.Any())
                return null;

            // Avoid duplicated keys and names
            if (objects.ContainsKey(key) || objects.Any(o => o.Value.Name == obj.Name)
                || otherObjects.ContainsKey(key) || otherObjects.Any(o => o.Value.Name == obj.Name)
                || schemaObjects.ContainsKey(key) || schemaObjects.Any(o => o.Value.Name == obj.Name))
            {
                if (UniquenessHelper.HasSameProperties(obj, objects, key, otherObjects, schemaObjects))
                    return key;

                obj.Name = UniquenessHelper.GetUniqueName(objects, obj.Name, otherObjects, schemaObjects);
                key = UniquenessHelper.GetUniqueKey(objects, key, otherObjects);
            }

            objects.Add(key, obj);
            return key;
        }

        private IList<Property> ParseSchema(IDictionary<string, Newtonsoft.JsonV4.Schema.JsonSchema> schema, IDictionary<string, ApiObject> objects, IDictionary<string, ApiEnum> enums, Newtonsoft.JsonV4.Schema.JsonSchema parentSchema)
        {
            var props = new List<Property>();
            if (schema == null)
                return props;

            foreach (var kv in schema)
            {
                var isEnum = kv.Value.Enum != null && kv.Value.Enum.Any();

                var enumName = string.Empty;
                if (isEnum)
                {
                    enumName = ParseEnum(kv.Key, kv.Value, enums, kv.Value.Description);
                }

                var prop = CreateProperty(parentSchema, kv, isEnum, enumName);

                ParseComplexTypes(objects, null, kv.Value, prop, kv, enums);
                props.Add(prop);
            }

            return props;
        }

        private void ParseArray(IDictionary<string, ApiObject> objects, Newtonsoft.JsonV4.Schema.JsonSchema schema, Property prop, KeyValuePair<string, Newtonsoft.JsonV4.Schema.JsonSchema> property, IDictionary<string, ApiEnum> enums)
        {
            var netType = NetTypeMapper.GetNetType(schema.Items.First().Type, schema.Items.First().Format);

            if (netType != null)
            {
                prop.Type = CollectionTypeHelper.GetCollectionType(netType);
            }
            else
            {
                prop.Type = CollectionTypeHelper.GetCollectionType(NetNamingMapper.GetObjectName(property.Key));
                foreach (var item in schema.Items)
                {
                    var key = ParseObject(property.Key, item, objects, enums, item);
                    if(key != null)
                        prop.Type = CollectionTypeHelper.GetCollectionType(NetNamingMapper.GetObjectName(key));
                }
            }
        }

        private void ParseProperties(IDictionary<string, ApiObject> objects, ICollection<Property> props, IDictionary<string, JsonSchema> properties, IDictionary<string, ApiEnum> enums)
        {
            if (properties == null)
                return;

            foreach (var property in properties)
            {
                if ( (property.Value.Enum != null && !property.Value.Enum.Any()) && (property.Value.Type == null || property.Value.Type == JsonSchemaType.Null 
                    || property.Value.Type == JsonSchemaType.None))
                    continue;

                var key = property.Key;
                if (string.IsNullOrWhiteSpace(key))
                    key = UniquenessHelper.GetUniqueName(props);

                var isEnum = property.Value.Enum != null && property.Value.Enum.Any();

                var enumName = string.Empty;
                if (isEnum)
                {
                    enumName = ParseEnum(key, property.Value, enums, property.Value.Description);
                }

                var type = GetType(property, isEnum, enumName);
                if (type == null)
                    continue;

                var prop = CreateProperty(key, type, property, isEnum);


                ParseComplexTypes(objects, property.Value, prop, property, key, enums);
                props.Add(prop);

                //AdditionalProperties(props, property.Value);
            }
        }

        private string ParseEnum(string key, JsonSchema schema, IDictionary<string, ApiEnum> enums, string description)
        {
            var name = NetNamingMapper.GetObjectName(key);

            var apiEnum = new ApiEnum
            {
                Name = name,
                Description = description,
                Values = schema.Enum.Select(e => RamlTypeParser.ToEnumValueName(e.ToString(), name)).ToList()
            };

            if (enums.ContainsKey(name))
            {
                if (IsAlreadyAdded(enums, apiEnum))
                    return name;

                apiEnum.Name = UniquenessHelper.GetUniqueName(enums, name);
            }

            enums.Add(apiEnum.Name, apiEnum);

            return apiEnum.Name;
        }

        private string ParseEnum(string key, Newtonsoft.JsonV4.Schema.JsonSchema schema, IDictionary<string, ApiEnum> enums, string description)
        {
            var name = NetNamingMapper.GetObjectName(key);

            var apiEnum = new ApiEnum
            {
                Name = name,
                Description = description,
                Values = schema.Enum.Select(e => RamlTypeParser.ToEnumValueName(e.ToString(), name)).ToList()
            };

            if (enums.ContainsKey(name))
            {
                if(IsAlreadyAdded(enums, apiEnum))
                    return name;

                apiEnum.Name = UniquenessHelper.GetUniqueName(enums, name);
            }

            enums.Add(apiEnum.Name, apiEnum);

            return apiEnum.Name;
        }

        private bool IsAlreadyAdded(IDictionary<string, ApiEnum> enums, ApiEnum apiEnum)
        {
            foreach (var @enum in enums)
            {
                if(apiEnum.Values.Count != @enum.Value.Values.Count)
                    continue;

                if (apiEnum.Values.Any(x => !@enum.Value.Values.Any(y => y.OriginalName == x.OriginalName)))
                    continue;

                return true;
            }
            return false;
        }


        private void ParseComplexTypes(IDictionary<string, ApiObject> objects, JsonSchema schema, Property prop, KeyValuePair<string, JsonSchema> property, string key, IDictionary<string, ApiEnum> enums)
        {
            if (schema.Type.HasValue && (schema.Type == JsonSchemaType.Object || schema.Type.Value.ToString().Contains("Object")))
            {
                if (!string.IsNullOrWhiteSpace(schema.Id) && ids.Contains(schema.Id))
                        return;
                
                if (!string.IsNullOrWhiteSpace(schema.Id))
                    ids.Add(schema.Id);

                var type = string.IsNullOrWhiteSpace(property.Value.Id) ? key : property.Value.Id;
                type = ParseObject(type, schema, objects, enums);
                if(type != null)
                    prop.Type = NetNamingMapper.GetObjectName(type);

                return;
            }


            if (schema.Type == JsonSchemaType.Array)
                ParseArray(objects, schema, prop, property, enums);

        }

        private void ParseArray(IDictionary<string, ApiObject> objects, JsonSchema schema, Property prop, KeyValuePair<string, JsonSchema> property, IDictionary<string, ApiEnum> enums)
        {
            if (schema.Items == null || !schema.Items.Any())
                return;

            var netType = NetTypeMapper.GetNetType(schema.Items.First().Type, property.Value.Format);

            if (netType != null)
            {
                prop.Type = CollectionTypeHelper.GetCollectionType(netType);
            }
            else
            {
                prop.Type = CollectionTypeHelper.GetCollectionType(NetNamingMapper.GetObjectName(property.Key));
                foreach (var item in schema.Items)
                {
                    var modifiedKey = ParseObject(property.Key, item, objects, enums);
                    if (modifiedKey != null)
                        prop.Type = CollectionTypeHelper.GetCollectionType(NetNamingMapper.GetObjectName(modifiedKey));
                }
            }
        }


    }
}