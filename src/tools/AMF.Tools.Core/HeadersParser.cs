using System.Collections.Generic;
using AMF.Api.Core;
using AMF.Parser.Model;
using AMF.Tools.Core.ClientGenerator;

namespace AMF.Tools.Core
{
    public class HeadersParser
    {
        public static ApiObject GetHeadersObject(ClientGeneratorMethod generatedMethod, Operation method, string objectName)
        {
            return new ApiObject
            {
                Name = generatedMethod.Name + objectName + "Header",
                Properties = ParseHeaders(method)
            };
        }

        public static ApiObject GetHeadersObject(ClientGeneratorMethod generatedMethod, Response response, string objectName)
        {
           return new ApiObject
            {
                Name = generatedMethod.Name + objectName + ParserHelpers.GetStatusCode(response.StatusCode) + "ResponseHeader",
                Properties = ParseHeaders(response)
            };
        }

        public static IList<Property> ParseHeaders(Operation method)
        {
            return ConvertHeadersToProperties(method.Request.Headers);
        }

        public static IList<Property> ParseHeaders(Response response)
        {
            return ConvertHeadersToProperties(response.Headers);
        }

        public static IList<Property> ConvertHeadersToProperties(IEnumerable<Parameter> headers)
        {
            var properties = new List<Property>();
            if (headers == null)
            {
                return properties;
            }

            foreach (var header in headers)
            {
                var description = ParserHelpers.RemoveNewLines(header.Description);

                var shape = (ScalarShape)header.Schema;
                var type = NewNetTypeMapper.GetNetType(shape);
                var typeSuffix = (type == "string" || header.Required ? "" : "?");

                properties.Add(new Property
                               {
                                   Type = type + typeSuffix,
                                   Name = NetNamingMapper.GetPropertyName(header.Name),
                                   OriginalName = shape.DisplayName,
                                   Description = description,
                                   Example = ObjectParser.MapExample(shape),
                                   Required = header.Required
                               });
            }
            return properties;
        }
    }
}