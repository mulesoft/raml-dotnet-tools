using System.Collections.Generic;
using System.Linq;
using RAML.Parser.Model;
using AMF.Tools.Core.Pluralization;
using RAML.Api.Core;
using System;

namespace AMF.Tools.Core
{
    public class RequestTypesService
    {
        private readonly IDictionary<string, ApiObject> schemaObjects;
        private readonly IDictionary<string, ApiObject> schemaRequestObjects;
        private readonly IDictionary<string, string> linkKeysWithObjectNames;
        private readonly IDictionary<string, ApiEnum> enums;
        private readonly SchemaParameterParser schemaParameterParser = new SchemaParameterParser(new EnglishPluralizationService());

        public RequestTypesService(IDictionary<string, ApiObject> schemaObjects, IDictionary<string, ApiObject> schemaRequestObjects, 
            IDictionary<string, string> linkKeysWithObjectNames, IDictionary<string, ApiEnum> enums)
        {
            this.schemaObjects = schemaObjects;
            this.schemaRequestObjects = schemaRequestObjects;
            this.linkKeysWithObjectNames = linkKeysWithObjectNames;
            this.enums = enums;
        }

        public GeneratorParameter GetRequestParameter(string key, Operation method, EndPoint resource, string fullUrl, IEnumerable<string> defaultMediaTypes)
        {
            if (method.Request == null || !method.Request.Payloads.Any())
                return new GeneratorParameter { Name = "content", Type = "string" };

            var mimeType = GetMimeType(method.Request.Payloads, defaultMediaTypes);
            var type = NewNetTypeMapper.GetNetType(mimeType, schemaObjects, schemaRequestObjects, enums);
            if (RamlTypesHelper.IsPrimitiveOrSchemaObject(type, schemaObjects) || RamlTypesHelper.IsPrimitiveOrSchemaObject(type, schemaRequestObjects))
            {
                return new GeneratorParameter
                {
                    Name = string.IsNullOrWhiteSpace(mimeType.Name) ? GetParameterName(type) : GetParameterName(mimeType.Name),
                    Description = mimeType.Description,
                    Type = type
                };
            }

            //var apiObjectByKey = GetRequestApiObjectByKey(key);
            //if (apiObjectByKey != null)
            //    return CreateGeneratorParameter(apiObjectByKey);

            //apiObjectByKey = GetRequestApiObjectByKey(NetNamingMapper.GetObjectName(key));
            //if (apiObjectByKey != null)
            //    return CreateGeneratorParameter(apiObjectByKey);

            var requestKey = key + GeneratorServiceBase.RequestContentSuffix;
            //apiObjectByKey = GetRequestApiObjectByKey(requestKey);
            //if (apiObjectByKey != null)
            //    return CreateGeneratorParameter(apiObjectByKey);
            ApiObject apiObjectByKey;

            if (linkKeysWithObjectNames.ContainsKey(key))
            {
                var linkedKey = linkKeysWithObjectNames[key];
                apiObjectByKey = GetRequestApiObjectByKey(linkedKey);
                if (apiObjectByKey != null)
                    return CreateGeneratorParameter(apiObjectByKey);
            }

            if (linkKeysWithObjectNames.ContainsKey(requestKey))
            {
                var linkedKey = linkKeysWithObjectNames[requestKey];
                apiObjectByKey = GetRequestApiObjectByKey(linkedKey);
                if (apiObjectByKey != null)
                    return CreateGeneratorParameter(apiObjectByKey);
            }

            return new GeneratorParameter { Name = "content", Type = "string" };
        }

        private string GetParameterName(string name)
        {
            var res = NetNamingMapper.GetObjectName(name);
            res = res.Substring(0, 1).ToLowerInvariant() + res.Substring(1);
            return res;
        }

        private GeneratorParameter CreateGeneratorParameter(ApiObject apiObject)
        {
            var generatorParameter = new GeneratorParameter
            {
                Name = apiObject.Name.ToLower(),
                Type = RamlTypesHelper.GetTypeFromApiObject(apiObject),
                Description = apiObject.Description
            };
            return generatorParameter;
        }

        private ApiObject GetRequestApiObjectByKey(string key)
        {
            if (!RequestHasKey(key))
                return null;

            return schemaObjects.ContainsKey(key) ? schemaObjects[key] : schemaRequestObjects[key];
        }

        private bool RequestHasKey(string key)
        {
            return schemaObjects.ContainsKey(key) || schemaRequestObjects.ContainsKey(key);
        }

        private Shape GetMimeType(IEnumerable<Payload> body, IEnumerable<string> defaultMediaTypes)
        {
            var isDefaultMediaTypeDefined = defaultMediaTypes.Any();
            var hasSchemaWithDefaultMediaType = body.Any(b => defaultMediaTypes.Any(m => m.ToLowerInvariant() == b.MediaType.ToLowerInvariant()) 
            && b.Schema != null);

            if (isDefaultMediaTypeDefined && hasSchemaWithDefaultMediaType)
            {
                foreach (var mediaType in defaultMediaTypes)
                {
                    if(body.Any(b => b.MediaType.ToLowerInvariant() == mediaType && b.Schema != null))
                        return body.First(b => b.MediaType.ToLowerInvariant() == mediaType && b.Schema != null).Schema;
                }
            }

            // if no default media types defined, use json
            if (body.Any(b => b.Schema != null && b.MediaType.ToLowerInvariant().Contains("json")))
                return body.First(b => b.Schema != null && b.MediaType.ToLowerInvariant().Contains("json")).Schema;

            // if no default and no json use first
            if (body.Any(b => b.Schema != null))
                return body.First(b => b.Schema != null).Schema;

            return null;
        }

    }
}