using System.Collections.Generic;
using System.Linq;
using Raml.Common;
using Raml.Parser.Expressions;
using Raml.Tools.Pluralization;

namespace Raml.Tools
{
    public class ResponseTypesService
    {
        private readonly IDictionary<string, ApiObject> schemaObjects;
        private readonly IDictionary<string, ApiObject> schemaResponseObjects;
        private readonly IDictionary<string, string> linkKeysWithObjectNames;
        private readonly IEnumerable<IDictionary<string, ResourceType>> resourceTypes;
        private readonly SchemaParameterParser schemaParameterParser = new SchemaParameterParser(new EnglishPluralizationService());

        public ResponseTypesService(IDictionary<string, ApiObject> schemaObjects, IDictionary<string, ApiObject> schemaResponseObjects, 
            IDictionary<string, string> linkKeysWithObjectNames, IEnumerable<IDictionary<string, ResourceType>> resourceTypes)
        {
            this.schemaObjects = schemaObjects;
            this.schemaResponseObjects = schemaResponseObjects;
            this.linkKeysWithObjectNames = linkKeysWithObjectNames;
            this.resourceTypes = resourceTypes;
        }

        public string GetResponseType(Method method, Resource resource, MimeType mimeType, string key, string responseCode, string fullUrl)
        {
            string returnType = null;
            if (mimeType.InlineType == null)
            {
                returnType = GetNamedReturnType(method, resource, mimeType, fullUrl);

                if (!string.IsNullOrWhiteSpace(returnType))
                    return returnType;

                returnType = GetReturnTypeFromResourceType(method, resource, key, responseCode, fullUrl);

                if (!string.IsNullOrWhiteSpace(returnType))
                    return returnType;
            }

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
                if (NetTypeMapper.IsPrimitiveType(subtype))
                    subtype = NetTypeMapper.Map(subtype);
                else
                    subtype = NetNamingMapper.GetObjectName(subtype);

                return CollectionTypeHelper.GetCollectionType(CollectionTypeHelper.GetCollectionType(subtype));
            }

            if (type.EndsWith("[]")) // array
            {
                var subtype = type.Substring(0, type.Length - 2);
                if (NetTypeMapper.IsPrimitiveType(subtype))
                    subtype = NetTypeMapper.Map(subtype);
                else
                    subtype = NetNamingMapper.GetObjectName(subtype);

                return CollectionTypeHelper.GetCollectionType(subtype);
            }

            if (type.EndsWith("{}")) // Map
            {
                var subtype = type.Substring(0, type.Length - 2);
                var netType = NetTypeMapper.Map(subtype);
                if (!string.IsNullOrWhiteSpace(netType))
                    return "IDictionary<string, " + netType + ">";

                var objectType = GetReturnTypeFromName(subtype);
                if (!string.IsNullOrWhiteSpace(objectType))
                    return "IDictionary<string, " + objectType + ">";

                return "IDictionary<string, object>";
            }

            if (NetTypeMapper.IsPrimitiveType(type))
                return NetTypeMapper.Map(type);

            return type;
        }




        private string GetReturnTypeFromResourceType(Method method, Resource resource, string key, string responseCode, string fullUrl)
        {
            var returnType = string.Empty;
            if (resource.Type == null || !resource.Type.Any() ||
                !resourceTypes.Any(rt => rt.ContainsKey(resource.GetSingleType())))
                return returnType;

            var verb = RamlTypesHelper.GetResourceTypeVerb(method, resource, resourceTypes);
            if (verb == null || verb.Responses == null
                || !verb.Responses.Any(r => r != null && r.Body != null
                                            && r.Body.Values.Any(m => !string.IsNullOrWhiteSpace(m.Schema) && !string.IsNullOrWhiteSpace(m.Type))))
                return returnType;

            var response = verb.Responses.FirstOrDefault(r => r.Code == responseCode);
            if (response == null)
                return returnType;

            var resourceTypeMimeType = GeneratorServiceHelper.GetMimeType(response);
            if (resourceTypeMimeType != null)
            {
                returnType = GetReturnTypeFromResponseWithoutCheckingResourceTypes(method, resource, resourceTypeMimeType, key, responseCode, fullUrl);
            }
            return returnType;
        }

        // avoids infinite recursion
        private string GetReturnTypeFromResponseWithoutCheckingResourceTypes(Method method, Resource resource, MimeType mimeType, string key, string responseCode, string fullUrl)
        {
            var returnType = GetNamedReturnType(method, resource, mimeType, fullUrl);

            if (!string.IsNullOrWhiteSpace(returnType))
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

            return returnType;
        }

        private string GetNamedReturnType(Method method, Resource resource, MimeType mimeType, string fullUrl)
        {
            if (mimeType.Schema != null && mimeType.Schema.Contains("<<") && mimeType.Schema.Contains(">>"))
                return GetReturnTypeFromParameter(method, resource, fullUrl, mimeType.Schema);

            if (mimeType.Schema != null && !mimeType.Schema.Contains("<") && !mimeType.Schema.Contains("{"))
                return GetReturnTypeFromName(mimeType.Schema);

            if (!string.IsNullOrWhiteSpace(mimeType.Type))
            {
                if (mimeType.Type.Contains("<<") && mimeType.Type.Contains(">>"))
                    return GetReturnTypeFromParameter(method, resource, fullUrl, mimeType.Type);

                var type = GetReturnTypeFromName(mimeType.Type);
                if (!string.IsNullOrWhiteSpace(type))
                    return type;

                return DecodeResponseRaml1Type(mimeType.Type);
            }

            return string.Empty;
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

        private string GetReturnTypeFromParameter(Method method, Resource resource, string fullUrl, string schema)
        {
            var type = schemaParameterParser.Parse(schema, resource, method, fullUrl);

            if (schemaObjects.Values.Any(o => o.Name.ToLowerInvariant() == type.ToLowerInvariant()))
            {
                var apiObject = schemaObjects.Values
                    .First(o => o.Name.ToLowerInvariant() == type.ToLowerInvariant());
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }


            if (schemaResponseObjects.Values.Any(o => o.Name.ToLowerInvariant() == type.ToLowerInvariant()))
            {
                var apiObject = schemaResponseObjects.Values
                    .First(o => o.Name.ToLowerInvariant() == type.ToLowerInvariant());
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }

            return string.Empty;
        }


        private bool ResponseHasKey(string key)
        {
            return schemaObjects.ContainsKey(key) || schemaResponseObjects.ContainsKey(key);
        }

        private string GetReturnTypeFromResponseByKey(string key)
        {
            var apiObject = schemaObjects.ContainsKey(key) ? schemaObjects[key] : schemaResponseObjects[key];

            return RamlTypesHelper.GetTypeFromApiObject(apiObject);
        }
        
    }
}