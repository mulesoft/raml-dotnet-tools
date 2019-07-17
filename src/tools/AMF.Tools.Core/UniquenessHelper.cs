using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core
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
            for (var i = 0; i < 200; i++)
            {
                var unique = name + i;
                if (schemaObjects.All(p => p.Value.Name != unique) && objects.All(p => p.Value.Name != unique) && otherObjects.All(p => p.Value.Name != unique))
                    return unique;
            }

            foreach (var suffix in Suffixes)
            {
                for (var i = 0; i < 200; i++)
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

        public static string GetUniqueName(string name, IDictionary<string, ApiEnum> existingEnums, IDictionary<string, ApiEnum> newEnums)
        {
            for (var i = 0; i < 100; i++)
            {
                var unique = name + i;
                if (existingEnums.All(p => p.Value.Name != unique) && newEnums.All(p => p.Value.Name != unique))
                    return unique;
            }

            foreach (var suffix in Suffixes)
            {
                for (var i = 0; i < 100; i++)
                {
                    var unique = name + suffix + i;
                    if (existingEnums.All(p => p.Value.Name != unique) && newEnums.All(p => p.Value.Name != unique))
                        return unique;
                }
            }
            throw new InvalidOperationException("Could not find a unique name for enum: " + name);
        }

        internal static bool HasSamevalues(ApiEnum apiEnum, IDictionary<string, ApiEnum> existingEnums, IDictionary<string, ApiEnum> newEnums)
        {
            var matchName = existingEnums.FirstOrDefault(e => e.Value.Name == apiEnum.Name).Value;
            if (matchName == null)
                matchName = newEnums.FirstOrDefault(e => e.Value.Name == apiEnum.Name).Value;

            if (matchName == null)
                return false;

            if (matchName.Values.Count != apiEnum.Values.Count)
                return false;

            foreach(var val in matchName.Values)
            {
                if (!apiEnum.Values.Any(v => v.OriginalName == val.OriginalName))
                    return false;
            }
            return true;
        }

        public static IEnumerable<ApiObject> FindAllObjectsWithSameProperties(ApiObject apiObject, string key, IDictionary<string, ApiObject> objects,
             IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            var objs = FindAllObjects(apiObject, schemaObjects, key);
            objs.AddRange(FindAllObjects(apiObject, objects, key));
            objs.AddRange(FindAllObjects(apiObject, otherObjects, key));
            return objs;
        }


        public static ApiObject FindObjectByKeyNameOrType(ApiObject apiObject, string key, IDictionary<string, ApiObject> objects,
             IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            var obj = FindObject(apiObject, schemaObjects, key);

            if (obj == null)
                obj = FindObject(apiObject, objects, key);

            if (obj == null)
                obj = FindObject(apiObject, otherObjects, key);

            return obj;
        }

        public static bool AnyWithSameProperties(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key, IDictionary<string, ApiObject> otherObjects,
            IDictionary<string, ApiObject> schemaObjects)
        {
            var objs = FindAllObjectsWithSameProperties(apiObject, key, objects, otherObjects, schemaObjects);

            if (objs.Any(obj => apiObject.Properties.All(property => obj.Properties.Any(p => p.Name == property.Name && p.Type == property.Type))))
                return true;

            return false;
        }

        public static ApiObject FirstOrDefaultWithSameProperties(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key, 
            IDictionary<string, ApiObject> otherObjects, IDictionary<string, ApiObject> schemaObjects)
        {
            var objs = FindAllObjectsWithSameProperties(apiObject, key, objects, otherObjects, schemaObjects);
            return objs.FirstOrDefault(o => apiObject.Properties.All(property => o.Properties.Any(p => p.Name == property.Name && p.Type == property.Type)));
        }


        public static bool HasSameProperties(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key, IDictionary<string, ApiObject> otherObjects, 
            IDictionary<string, ApiObject> schemaObjects)
        {
            var obj = FindObjectByKeyNameOrType(apiObject, key, objects, otherObjects, schemaObjects);

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

        //private static ApiObject FindObject(IHasName apiObject, IDictionary<string, ApiObject> objects, string key)
        //{
        //    var foundKey = objects.Keys.FirstOrDefault(k => string.Equals(k, key));
        //    if (foundKey != null)
        //        return objects[foundKey];

        //    var byName = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Name == apiObject.Name);
        //    if (objects.Any(byName))
        //        return objects.First(byName).Value;

        //    return null;
        //}

        private static ApiObject FindObject(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key)
        {
            var foundKey = objects.Keys.FirstOrDefault(k => string.Equals(k, key));
            if (foundKey != null)
                return objects[foundKey];

            var byName = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Name == apiObject.Name);
            if (objects.Any(byName))
                return objects.First(byName).Value;

            var byType = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Type == apiObject.Type);
            if (objects.Any(byType))
                return objects.First(byType).Value;

            return null;
        }

        private static List<ApiObject> FindAllObjects(ApiObject apiObject, IDictionary<string, ApiObject> objects, string key)
        {
            var found = new List<ApiObject>();

            if (!string.IsNullOrWhiteSpace(key))
            {
                var byKey = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Key != null && o.Value != null && o.Key.StartsWith(key)
                                                                        && o.Value.Properties.Count == apiObject.Properties.Count);
                if (objects.Any(byKey))
                    found.Add(objects.First(byKey).Value);
            }

            var byName = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Name != null && o.Value.Name.StartsWith(apiObject.Name) 
                                                                && o.Value.Properties.Count == apiObject.Properties.Count);
            if (objects.Any(byName))
                found.Add(objects.First(byName).Value);

            var byType = new Func<KeyValuePair<string, ApiObject>, bool>(o => o.Value.Type != null && o.Value.Type.StartsWith(apiObject.Type) 
                                                                         && o.Value.Properties.Count == apiObject.Properties.Count);
            if (objects.Any(byType))
                found.Add(objects.First(byType).Value);

            return found;
        }

    }
}