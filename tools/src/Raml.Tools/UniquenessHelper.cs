using System;
using System.Collections.Generic;
using System.Linq;
using Raml.Tools.WebApiGenerator;

namespace Raml.Tools
{
    public class UniquenessHelper
    {
        private static readonly string[] Suffixes = { "A", "B", "C", "D", "E", "F", "G" };

        public static string GetUniqueKey(IDictionary<string, ApiObject> objects, string key, IDictionary<string, ApiObject> otherObjects)
        {
            for (var i = 0; i < 100; i++)
            {
                var unique = key + i;
                if (objects.All(p => !string.Equals(p.Key, unique, StringComparison.OrdinalIgnoreCase))
                    && otherObjects.All(p => !string.Equals(p.Key, unique, StringComparison.OrdinalIgnoreCase)))
                    return unique;
            }

            foreach (var suffix in Suffixes)
            {
                for (var i = 0; i < 100; i++)
                {
                    var unique = key + suffix + i;
                    if (objects.All(p => !string.Equals(p.Key, unique, StringComparison.OrdinalIgnoreCase))
                        && otherObjects.All(p => !string.Equals(p.Key, unique, StringComparison.OrdinalIgnoreCase)))
                        return unique;
                }
            }
            throw new InvalidOperationException("Could not find a key name for object: " + key);
        }

        public static string GetUniqueName(IDictionary<string, ApiObject> objects, string name, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            for (var i = 0; i < 100; i++)
            {
                var unique = name + i;
                if (schemaObjects.All(p => p.Value.Name != unique) && objects.All(p => p.Value.Name != unique) && otherObjects.All(p => p.Value.Name != unique))
                    return unique;
            }

            foreach (var suffix in Suffixes)
            {
                for (var i = 0; i < 100; i++)
                {
                    var unique = name + suffix + i;
                    if (schemaObjects.All(p => p.Value.Name != unique) && objects.All(p => p.Value.Name != unique) && otherObjects.All(p => p.Value.Name != unique))
                        return unique;
                }
            }
            throw new InvalidOperationException("Could not find a unique name for object: " + name);
        }

        public static string GetUniqueName(ICollection<Property> props)
        {
            foreach (var suffix in Suffixes)
            {
                var unique = suffix;
                if (props.All(p => p.Name != unique))
                    return unique;
            }
            for (var i = 0; i < 100; i++)
            {
                var unique = "A" + i;
                if (props.All(p => p.Name != unique))
                    return unique;
            }
            throw new InvalidOperationException("Could not find a unique name for property");
        }

        public static string GetUniqueName(IDictionary<string, ApiEnum> enums, string name)
        {
            for (var i = 0; i < 100; i++)
            {
                var unique = name + i;
                if (enums.All(p => p.Key != unique))
                    return unique;
            }

            foreach (var suffix in Suffixes)
            {
                for (var i = 0; i < 100; i++)
                {
                    var unique = name + suffix + i;
                    if (enums.All(p => p.Key != unique))
                        return unique;
                }
            }
            throw new InvalidOperationException("Could not find a unique name for enum: " + name);
        }

        public static ApiObject FindObjectWithSameProperties(ApiObject apiObject, string key, IDictionary<string, ApiObject> objects,
             IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            var obj = FindObject(apiObject, schemaObjects, key);

            if (obj == null)
                obj = FindObject(apiObject, objects, key);

            if (obj == null)
                obj = FindObject(apiObject, otherObjects, key);

            return obj;
        }

        public static bool HasSameProperties(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key, IDictionary<string, ApiObject> otherObjects, 
            IDictionary<string, ApiObject> schemaObjects)
        {
            var obj = FindObjectWithSameProperties(apiObject, key, objects, otherObjects, schemaObjects);

            if (obj == null)
                throw new InvalidOperationException("Could not find object with key " + key);

            if (!string.IsNullOrWhiteSpace(obj.GeneratedCode))
            {
                if(objects.Any(o => o.Value.GeneratedCode == obj.GeneratedCode) 
                    || otherObjects.Any(o => o.Value.GeneratedCode == obj.GeneratedCode) 
                    || schemaObjects.Any(o => o.Value.GeneratedCode == obj.GeneratedCode))
                    return true;
            }

            if (obj.Properties.Count != apiObject.Properties.Count)
                return false;

            return apiObject.Properties.All(property => obj.Properties.Any(p => p.Name == property.Name && p.Type == property.Type));
        }

        // To use with XML Schema objects
        public static bool HasSameProperties(ApiObject apiObject, IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            return objects.Any(o => o.Value.GeneratedCode == apiObject.GeneratedCode)
                   || otherObjects.Any(o => o.Value.GeneratedCode == apiObject.GeneratedCode)
                   || schemaObjects.Any(o => o.Value.GeneratedCode == apiObject.GeneratedCode);
        }

        private static ApiObject FindObject(IHasName apiObject, IDictionary<string, ApiObject> objects, string key)
        {
            var foundKey = objects.Keys.FirstOrDefault(k => string.Equals(k, key));
            var obj = foundKey != null ? objects[foundKey] : objects.FirstOrDefault(o => o.Value.Name == apiObject.Name).Value;
            return obj;
        }

    }
}