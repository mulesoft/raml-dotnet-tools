using System.Collections.Generic;
using System.Linq;
using RAML.Parser.Model;
using RAML.Api.Core;
using System.Text.RegularExpressions;

namespace AMF.Tools.Core
{
    public class NewNetTypeMapper
    {
        private static readonly IDictionary<string, string> TypeStringConversion =
            new Dictionary<string, string>
            {
                {
                    "integer",
                    "int"
                },
                {
                    "string",
                    "string"
                },
                {
                    "boolean",
                    "bool"
                },
                {
                    "float",
                    "decimal"
                },
                {
                    "number",
                    "decimal"
                },
                {
                    "any",
                    "object"
                },
                {
                    "date",
                    "DateTime"
                },
                {
                    "datetime",
                    "DateTime"
                },
                {
                    "dateTime",
                    "DateTime"
                },
                {
                    "dateTimeOnly",
                    "DateTime"
                },
                {
                    "date-only",
                    "DateTime"
                },
                {
                    "time",
                    "DateTime"
                },
                {
                    "time-only",
                    "DateTime"
                },
                {
                    "datetime-only",
                    "DateTime"
                },
                {
                    "file",
                    "byte[]"
                }
            };

        private static readonly IDictionary<string, string> NumberFormatConversion = new Dictionary<string, string>
        {
            {"double", "double"},
            {"float", "float"},
            {"int16", "short"},
            {"short", "short"},
            {"int64", "long"},
            {"long", "long"},
            {"int32", "int"},
            {"int", "int"},
            {"int8", "byte"}
        };

        private static readonly IDictionary<string, string> DateFormatConversion = new Dictionary<string, string>
        {
            {"rfc3339", "DateTime"},
            {"rfc2616", "DateTimeOffset"}
        };

        public static string GetNetType(Shape shape, IDictionary<string, ApiObject> existingObjects = null,
            IDictionary<string, ApiObject> newObjects = null, IDictionary<string, ApiEnum> existingEnums = null, IDictionary<string, ApiEnum> newEnums = null)
        {
            if (!string.IsNullOrWhiteSpace(shape.LinkTargetName))
                return NetNamingMapper.GetObjectName(GetTypeFromLinkOrId(shape.LinkTargetName));

            string id = shape.Id;

            if (shape is ScalarShape scalar)
            {
                if(shape.Values != null && shape.Values.Any())
                {
                    var key = GetTypeFromLinkOrId(id);
                    if (existingEnums != null && existingEnums.ContainsKey(key))
                        return existingEnums[key].Name;
                    if (newEnums != null && newEnums.ContainsKey(key))
                        return newEnums[key].Name;
                }
                return GetNetType(scalar.DataType.Substring(scalar.DataType.LastIndexOf('#') + 1), scalar.Format);
            }

            string type = null;
            if (shape is ArrayShape arr && !(arr.Items is ScalarShape) && !string.IsNullOrWhiteSpace(arr.Items.Id))
                type = FindTypeById(arr.Items.Id, existingObjects, newObjects, existingEnums, newEnums);

            if (type != null)
                return CollectionTypeHelper.GetCollectionType(type);

            if (!(shape is ArrayShape) && !string.IsNullOrWhiteSpace(id))
                type = FindTypeById(id, existingObjects, newObjects, existingEnums, newEnums);

            if (type != null)
                return type;

            if (id != null && id.Contains("#/declarations"))
            {
                var key = GetTypeFromLinkOrId(id);
                if (existingObjects != null && (existingObjects.ContainsKey(key)
                    || existingObjects.Keys.Any(k => k.ToLowerInvariant() == key.ToLowerInvariant())))
                {
                    if (existingObjects.ContainsKey(key))
                        return existingObjects[key].Type;

                    return existingObjects.First(kv => kv.Key.ToLowerInvariant() == key.ToLowerInvariant()).Value.Type;
                }
                if (newObjects != null && newObjects.ContainsKey(key))
                    return newObjects[key].Type;
            }

            if (shape is ArrayShape array)
                return CollectionTypeHelper.GetCollectionType(GetNetType(array.Items, existingObjects, newObjects, existingEnums, newEnums));

            if (shape is FileShape file)
                return TypeStringConversion["file"];

            if (shape.Inherits.Count() == 1)
            {
                if (shape is NodeShape nodeShape)
                {
                    if (nodeShape.Properties.Count() == 0)
                        return GetNetType(nodeShape.Inherits.First(), existingObjects, newObjects, existingEnums, newEnums);
                }
                if (shape.Inherits.First() is ArrayShape arrayShape)
                    return CollectionTypeHelper.GetCollectionType(GetNetType(arrayShape.Items, existingObjects, newObjects, existingEnums, newEnums));

                if (shape is AnyShape any)
                {
                    var key = NetNamingMapper.GetObjectName(any.Inherits.First().Name);
                    if (existingObjects != null && existingObjects.ContainsKey(key))
                        return key;
                    if (newObjects != null && newObjects.ContainsKey(key))
                        return key;
                }
            }
            if (shape.Inherits.Count() > 0)
            {
                //TODO: check
            }

            if (shape.GetType() == typeof(AnyShape))
            {
                return GetNetType("any", null);
            }

            if (shape is UnionShape)
                return "object";

            return NetNamingMapper.GetObjectName(shape.Name);
        }

        private static string FindTypeById(string id, IDictionary<string, ApiObject> existingObjects = null,
            IDictionary<string, ApiObject> newObjects = null, IDictionary<string, ApiEnum> existingEnums = null, IDictionary<string, ApiEnum> newEnums = null)
        {
            if (existingObjects != null && existingObjects.ContainsKey(id))
                return existingObjects[id].Type;

            if (newObjects != null && newObjects.ContainsKey(id))
                return newObjects[id].Type;

            if (existingEnums != null && existingEnums.ContainsKey(id))
                return existingEnums[id].Name;

            if (newEnums != null && newEnums.ContainsKey(id))
                return newEnums[id].Name;

            return null;
        }

        private static string GetTypeFromLinkOrId(string linkOrId)
        {
            // workaround for rule exception??? in link target names in AMF js parser
            var regex = new Regex("/linked_[0-9]{1}");
            if (regex.IsMatch(linkOrId))
            {
                var name = regex.Replace(linkOrId, string.Empty);
                return name.Substring(name.LastIndexOf('/') + 1);
            }
            return linkOrId.Substring(linkOrId.LastIndexOf('/') + 1);
        }

        public static string GetNetType(string type, string format)
        {
            string netType;
            if (!string.IsNullOrWhiteSpace(format) &&
                (NumberFormatConversion.ContainsKey(format.ToLowerInvariant()) || DateFormatConversion.ContainsKey(format.ToLowerInvariant())))
            {
                netType = NumberFormatConversion.ContainsKey(format.ToLowerInvariant())
                    ? NumberFormatConversion[format.ToLowerInvariant()]
                    : DateFormatConversion[format.ToLowerInvariant()];
            }
            else
            {
                netType = Map(type);
            }
            return netType;
        }

        public static string Map(string type)
        {
            if (type != null)
                type = type.Trim();

            return !TypeStringConversion.ContainsKey(type) ? null : TypeStringConversion[type];
        }

        private static readonly string[] OtherPrimitiveTypes = { "double", "float", "byte", "short", "long", "DateTimeOffset" };

        public static bool IsPrimitiveType(string type)
        {
            type = type.Trim();

            if (type.EndsWith("?"))
                type = type.Substring(0, type.Length - 1);

            if (OtherPrimitiveTypes.Contains(type))
                return true;

            return TypeStringConversion.Any(t => t.Value == type) || TypeStringConversion.ContainsKey(type);
        }

        private static readonly string[] NotNullableTypes = new string[] { "int", "long", "double", "short", "byte", "float", "bool", "DateTime", "decimal" };
        public static bool IsNullableType(string type)
        {
            return !NotNullableTypes.Contains(type);
        }
    }
}