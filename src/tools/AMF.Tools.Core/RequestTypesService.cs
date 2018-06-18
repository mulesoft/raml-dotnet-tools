using System.Collections.Generic;
using System.Linq;
using AMF.Parser.Model;
using AMF.Tools.Core.Pluralization;
using AMF.Api.Core;

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
            if(method.Request == null || !method.Request.Payloads.Any())
                return new GeneratorParameter { Name = "content", Type = "string" }; //TODO: check

            var mimeType = GetMimeType(method.Request.Payloads, defaultMediaTypes);
            return new GeneratorParameter //TODO: check
            {
                Name = mimeType.Name,
                Description = mimeType.Description,
                Type = NewNetTypeMapper.GetNetType(mimeType, schemaObjects, null, enums)
            };

            //if (mimeType != null)
            //{
            //    if (mimeType.Type != "object" && !string.IsNullOrWhiteSpace(mimeType.Type))
            //    {
            //        var apiObject = GetRequestApiObjectWhenNamed(method, resource, mimeType.Type, fullUrl);
            //        if (apiObject != null)
            //        {
            //            var generatorParameter = new GeneratorParameter
            //            {
            //                Name = apiObject.Name.ToLower(),
            //                Type = GetParamType(mimeType, apiObject),
            //                //Type = DecodeRequestRaml1Type(RamlTypesHelper.GetTypeFromApiObject(apiObject)),
            //                Description = apiObject.Description
            //            };
            //            return generatorParameter;
            //        }
            //    }
            //    if (!string.IsNullOrWhiteSpace(mimeType.Schema))
            //    {
            //        var apiObject = GetRequestApiObjectWhenNamed(method, resource, mimeType.Schema, fullUrl);
            //        if (apiObject != null)
            //            return CreateGeneratorParameter(apiObject);                    
            //    }
            //}

            //if (resource.Type != null && resource.Type.Any() &&
            //    resourceTypes.Any(rt => rt.ContainsKey(resource.GetSingleType())))
            //{
            //    var verb = RamlTypesHelper.GetResourceTypeVerb(method, resource, resourceTypes);
            //    if (verb != null && verb.Body != null && !string.IsNullOrWhiteSpace(verb.Body.Schema))
            //    {
            //        var apiObject = GetRequestApiObjectWhenNamed(method, resource, verb.Body.Schema, fullUrl);
            //        if (apiObject != null)
            //            return CreateGeneratorParameter(apiObject);
            //    }

            //    if (verb != null && verb.Body != null && !string.IsNullOrWhiteSpace(verb.Body.Type))
            //    {
            //        var apiObject = GetRequestApiObjectWhenNamed(method, resource,  verb.Body.Type, fullUrl);
            //        if (apiObject != null)
            //        {
            //            var generatorParameter = new GeneratorParameter
            //            {
            //                Name = apiObject.Name.ToLower(),
            //                Type = DecodeRequestRaml1Type(RamlTypesHelper.GetTypeFromApiObject(apiObject)),
            //                Description = apiObject.Description
            //            };
            //            return generatorParameter;
            //        }

            //        return new GeneratorParameter { Name = "content", Type = DecodeRequestRaml1Type(verb.Body.Type) };
            //    }
            //}

            //var apiObjectByKey = GetRequestApiObjectByKey(key);
            //if (apiObjectByKey != null)
            //    return CreateGeneratorParameter(apiObjectByKey);


            //var requestKey = key + GeneratorServiceBase.RequestContentSuffix;
            //apiObjectByKey = GetRequestApiObjectByKey(requestKey);
            //if (apiObjectByKey != null)
            //    return CreateGeneratorParameter(apiObjectByKey);

            //if (linkKeysWithObjectNames.ContainsKey(key))
            //{
            //    var linkedKey = linkKeysWithObjectNames[key];
            //    apiObjectByKey = GetRequestApiObjectByKey(linkedKey);
            //    if (apiObjectByKey != null)
            //        return CreateGeneratorParameter(apiObjectByKey);
            //}

            //if (linkKeysWithObjectNames.ContainsKey(requestKey))
            //{
            //    var linkedKey = linkKeysWithObjectNames[requestKey];
            //    apiObjectByKey = GetRequestApiObjectByKey(linkedKey);
            //    if (apiObjectByKey != null)
            //        return CreateGeneratorParameter(apiObjectByKey);
            //}

            //if (mimeType != null)
            //{
            //    string type;
            //    if(!string.IsNullOrWhiteSpace(mimeType.Type))
            //        type = mimeType.Type;
            //    else
            //        type = mimeType.Schema;

            //    if (!string.IsNullOrWhiteSpace(type))
            //    {
            //        var raml1Type = DecodeRequestRaml1Type(type);

            //        if (!string.IsNullOrWhiteSpace(raml1Type))
            //            return new GeneratorParameter {Name = "content", Type = raml1Type};
            //    }
            //}

            //return new GeneratorParameter { Name = "content", Type = "string" };
        }

        //private string GetParamType(MimeType mimeType, ApiObject apiObject)
        //{
        //    if (mimeType.Type.Contains("<<") && mimeType.Type.Contains(">>"))
        //        return RamlTypesHelper.GetTypeFromApiObject(apiObject);

        //    return DecodeRequestRaml1Type(mimeType.Type);
        //}

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

        private string DecodeRequestRaml1Type(string type)
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

                var apiObject = GetRequestApiObjectByName(subtype);
                if (apiObject != null)
                    return "IDictionary<string, " + RamlTypesHelper.GetTypeFromApiObject(apiObject) + ">";

                return "IDictionary<string, object>";
            }

            if (NewNetTypeMapper.IsPrimitiveType(type))
                return NewNetTypeMapper.Map(type);

            if (CollectionTypeHelper.IsCollection(type))
                return type;

            return NetNamingMapper.GetObjectName(type);
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

        private ApiObject GetRequestApiObjectWhenNamed(Operation method, EndPoint resource, string type, string fullUrl)
        {
            if (type.Contains("<<") && type.Contains(">>"))
                return GetRequestApiObjectByParametrizedName(method, resource, type, fullUrl);

            if (!type.Contains("<") && !type.Contains("{"))
                return GetRequestApiObjectByName(type);

            return null;
        }

        private ApiObject GetRequestApiObjectByName(string schema)
        {
            var type = RamlTypesHelper.ExtractType(schema.ToLowerInvariant());

            if (schemaObjects.Values.Any(o => o.Name.ToLowerInvariant() == type))
                return schemaObjects.Values.First(o => o.Name.ToLowerInvariant() == type);

            if (schemaRequestObjects.Values.Any(o => o.Name.ToLowerInvariant() == type))
                return schemaRequestObjects.Values.First(o => o.Name.ToLowerInvariant() == type);

            return null;
        }



        private ApiObject GetRequestApiObjectByParametrizedName(Operation method, EndPoint resource, string schema, string fullUrl)
        {
            var type = schemaParameterParser.Parse(schema, resource, method, fullUrl);

            if (schemaObjects.Any(o => o.Key.ToLower() == type.ToLowerInvariant()))
                return schemaObjects.First(o => o.Key.ToLower() == type.ToLowerInvariant()).Value;

            if (schemaObjects.Values.Any(o => o.Name.ToLower() == type.ToLowerInvariant()))
                return schemaObjects.Values.First(o => o.Name.ToLower() == type.ToLowerInvariant());

            if (schemaRequestObjects.Any(o => o.Key.ToLower() == type.ToLowerInvariant()))
                return schemaRequestObjects.First(o => o.Key.ToLower() == type.ToLowerInvariant()).Value;

            if (schemaRequestObjects.Values.Any(o => o.Name.ToLower() == type.ToLowerInvariant()))
                return schemaRequestObjects.Values.First(o => o.Name.ToLower() == type.ToLowerInvariant());

            return null;
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