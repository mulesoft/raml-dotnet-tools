using System;
using System.Collections.Generic;
using System.Linq;
using Raml.Common;
using AMF.Parser.Model;

namespace AMF.Tools.Core
{
    public class RamlTypesHelper
    {
        public static string DecodeRaml1Type(string type)
        {
            // TODO: can I handle this better ?
            if (type.Contains("(") || type.Contains("|"))
                return "object";

            if (type.EndsWith("[][]")) // array of arrays
            {
                var strippedType = type.Substring(0, type.Length - 4);
                if (NewNetTypeMapper.Map(strippedType) == null)
                    strippedType = NetNamingMapper.GetObjectName(strippedType);

                var decodeRaml1Type = CollectionTypeHelper.GetCollectionType(CollectionTypeHelper.GetCollectionType(strippedType));
                return decodeRaml1Type;
            }

            if (type.EndsWith("[]")) // array
            {
                var strippedType = type.Substring(0, type.Length - 2);

                if (NewNetTypeMapper.Map(strippedType) == null)
                    strippedType = NetNamingMapper.GetObjectName(strippedType);

                var decodeRaml1Type = CollectionTypeHelper.GetCollectionType(strippedType);
                return decodeRaml1Type;
            }

            if (type.EndsWith("{}")) // Map
            {
                var subtype = type.Substring(0, type.Length - 2);
                var netType = NewNetTypeMapper.Map(subtype);
                if (netType != null)
                    return "IDictionary<string, " + netType + ">";

               return "IDictionary<string, " + NetNamingMapper.GetObjectName(subtype) + ">";
            }

            if (CollectionTypeHelper.IsCollection(type))
                return type;

            return NetNamingMapper.GetObjectName(type);
        }

        public static string GetTypeFromApiObject(ApiObject apiObject)
        {
            if (apiObject.IsArray)
            {
                if (string.IsNullOrWhiteSpace(apiObject.Type))
                    return CollectionTypeHelper.GetCollectionType(apiObject.Name);

                if (CollectionTypeHelper.IsCollection(apiObject.Type))
                    return apiObject.Type;

                return CollectionTypeHelper.GetCollectionType(apiObject.Type);
            }
            if (apiObject.IsMap)
                return apiObject.Type;

            if (apiObject.IsScalar)
                return apiObject.Properties.First().Type;

            if (!string.IsNullOrWhiteSpace(apiObject.Type) && apiObject.Type != apiObject.Name)
                return apiObject.Type;

            return apiObject.Name;
        }

        //public static Verb GetResourceTypeVerb(Operation method, EndPoint resource, IEnumerable<IDictionary<string, ResourceType>> rootResourceTypes)
        //{
        //    var resourceTypes = rootResourceTypes.First(rt => rt.ContainsKey(resource.GetSingleType()));
        //    var resourceType = resourceTypes[resource.GetSingleType()];
        //    Verb verb;
        //    switch (method.Verb)
        //    {
        //        case "get":
        //            verb = resourceType.Get;
        //            break;
        //        case "delete":
        //            verb = resourceType.Delete;
        //            break;
        //        case "post":
        //            verb = resourceType.Post;
        //            break;
        //        case "put":
        //            verb = resourceType.Put;
        //            break;
        //        case "patch":
        //            verb = resourceType.Patch;
        //            break;
        //        case "options":
        //            verb = resourceType.Options;
        //            break;
        //        default:
        //            throw new InvalidOperationException("Verb not found " + method.Verb);
        //    }
        //    return verb;
        //}

        public static string ExtractType(string type)
        {
            if (type.EndsWith("[][]")) // array of arrays
                return type.Substring(0, type.Length - 4);

            if (type.EndsWith("[]")) // array
                return type.Substring(0, type.Length - 2);

            if (type.EndsWith("{}")) // Map
                return type.Substring(0, type.Length - 2);

            return type;
        }

    }
}