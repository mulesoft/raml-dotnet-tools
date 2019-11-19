using System.Collections.Generic;
using System.Linq;
using RAML.Parser.Model;
using AMF.Tools.Core.Pluralization;
using RAML.Api.Core;

namespace AMF.Tools.Core
{
    public class ResponseTypesService
    {
        private readonly IDictionary<string, ApiObject> schemaObjects;
        private readonly IDictionary<string, ApiObject> schemaResponseObjects;
        private readonly IDictionary<string, string> linkKeysWithObjectNames;
        private readonly IDictionary<string, ApiEnum> enums;
        private readonly SchemaParameterParser schemaParameterParser = new SchemaParameterParser(new EnglishPluralizationService());

        public ResponseTypesService(IDictionary<string, ApiObject> schemaObjects, IDictionary<string, ApiObject> schemaResponseObjects, 
            IDictionary<string, string> linkKeysWithObjectNames, IDictionary<string, ApiEnum> enums)
        {
            this.schemaObjects = schemaObjects;
            this.schemaResponseObjects = schemaResponseObjects;
            this.linkKeysWithObjectNames = linkKeysWithObjectNames;
            this.enums = enums;
        }

        public string GetResponseType(Operation method, EndPoint resource, Payload mimeType, string key, string responseCode, string fullUrl)
        {
            var returnType = GetNamedReturnType(method, resource, mimeType, fullUrl);
            if (!string.IsNullOrWhiteSpace(returnType) && (RamlTypesHelper.IsPrimitiveOrSchemaObject(returnType, schemaObjects)
                || RamlTypesHelper.IsPrimitiveOrSchemaObject(returnType, schemaResponseObjects)))
                return returnType;

            if (ResponseHasKey(key))
                return GetReturnTypeFromResponseByKey(key);

            var responseKey = key + ParserHelpers.GetStatusCode(responseCode) + GeneratorServiceBase.ResponseContentSuffix;
            if (ResponseHasKey(responseKey))
                return GetReturnTypeFromResponseByKey(responseKey);

            if (linkKeysWithObjectNames.ContainsKey(key))
            {
                var linkedKey = linkKeysWithObjectNames[key];
                if (ResponseHasKey(linkedKey))
                    return GetReturnTypeFromResponseByKey(linkedKey);
            }

            if (linkKeysWithObjectNames.ContainsKey(responseKey))
            {
                var linkedKey = linkKeysWithObjectNames[responseKey];
                if (ResponseHasKey(linkedKey))
                    return GetReturnTypeFromResponseByKey(linkedKey);
            }

            returnType = DecodeResponseRaml1Type(returnType);
            return returnType;
        }

        private string DecodeResponseRaml1Type(string type)
        {
            // TODO: can I handle this better ?
            if (type.Contains("(") || type.Contains("|"))
                return "string";

            if (type.EndsWith("[][]")) // array of arrays
            {
                var subtype = type.Substring(0, type.Length - 4);
                if (NewNetTypeMapper.IsPrimitiveType(subtype))
                    subtype = NewNetTypeMapper.Map(subtype);
                else
                    subtype = NetNamingMapper.GetObjectName(subtype);

                return CollectionTypeHelper.GetCollectionType(CollectionTypeHelper.GetCollectionType(subtype));
            }

            if (type.EndsWith("[]")) // array
            {
                var subtype = type.Substring(0, type.Length - 2);
                if (NewNetTypeMapper.IsPrimitiveType(subtype))
                    subtype = NewNetTypeMapper.Map(subtype);
                else
                    subtype = NetNamingMapper.GetObjectName(subtype);

                return CollectionTypeHelper.GetCollectionType(subtype);
            }

            if (type.EndsWith("{}")) // Map
            {
                var subtype = type.Substring(0, type.Length - 2);
                var netType = NewNetTypeMapper.Map(subtype);
                if (!string.IsNullOrWhiteSpace(netType))
                    return "IDictionary<string, " + netType + ">";

                var objectType = GetReturnTypeFromName(subtype);
                if (!string.IsNullOrWhiteSpace(objectType))
                    return "IDictionary<string, " + objectType + ">";

                return "IDictionary<string, object>";
            }

            if (NewNetTypeMapper.IsPrimitiveType(type))
                return NewNetTypeMapper.Map(type);

            return type;
        }

        private string GetNamedReturnType(Operation method, EndPoint resource, Payload mimeType, string fullUrl)
        {
            return NewNetTypeMapper.GetNetType(mimeType.Schema, schemaObjects, schemaResponseObjects, enums);
        }

        private string GetReturnTypeFromName(string type)
        {
            var toLower = type.ToLowerInvariant();
            toLower = toLower.Replace(".", string.Empty);

            if (schemaObjects.Values.Any(o => o.Name.ToLowerInvariant() == toLower))
            {
                var apiObject = schemaObjects.Values.First(o => o.Name.ToLowerInvariant() == toLower);
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }

            if (schemaResponseObjects.Values.Any(o => o.Name.ToLowerInvariant() == toLower))
            {
                var apiObject = schemaResponseObjects.Values.First(o => o.Name.ToLowerInvariant() == toLower);
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }

            return string.Empty;
        }

        private bool ResponseHasKey(string key)
        {
            return schemaObjects.ContainsKey(key) || schemaResponseObjects.ContainsKey(key)
                || schemaObjects.ContainsKey(NetNamingMapper.GetObjectName(key)) 
                || schemaResponseObjects.ContainsKey(NetNamingMapper.GetObjectName(key));
        }

        private string GetReturnTypeFromResponseByKey(string key)
        {
            ApiObject apiObject = null;
            if (schemaObjects.ContainsKey(key))
                apiObject = schemaObjects[key];

            if (schemaResponseObjects.ContainsKey(key))
                apiObject = schemaResponseObjects[key];

            var nameKey = NetNamingMapper.GetObjectName(key);
            if (schemaObjects.ContainsKey(nameKey))
                apiObject = schemaObjects[nameKey];

            if (schemaResponseObjects.ContainsKey(nameKey))
                apiObject = schemaResponseObjects[nameKey];

            return RamlTypesHelper.GetTypeFromApiObject(apiObject);
        }
        
    }
}