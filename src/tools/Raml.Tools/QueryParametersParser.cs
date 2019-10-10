using System.Collections.Generic;
using System.Linq;
using Raml.Common;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools
{
    public class QueryParametersParser
    {
        private readonly IDictionary<string, ApiObject> schemaObjects;

        public QueryParametersParser(IDictionary<string, ApiObject> schemaObjects)
        {
            this.schemaObjects = schemaObjects;
        }

        public ApiObject GetQueryObject(ClientGeneratorMethod generatedMethod, Method method, string objectName)
        {
            var queryObject = new ApiObject { Name = generatedMethod.Name + objectName + "Query" };
            queryObject.Properties = ParseParameters(method);


            return queryObject;
        }

        public IList<Property> ParseParameters(Method method)
        {
            return ConvertParametersToProperties(method.QueryParameters);
        }

        public IList<Property> ConvertParametersToProperties(IEnumerable<KeyValuePair<string, Parameter>> parameters)
        {
            var properties = new List<Property>();
            foreach (var parameter in parameters.Where(parameter => parameter.Value != null && parameter.Value.Type != null))
            {
                var description = ParserHelpers.RemoveNewLines(parameter.Value.Description);

				properties.Add(new Property
				               {
					               Type = GetType(parameter.Value),
					               Name = NetNamingMapper.GetPropertyName(parameter.Key),
                                   OriginalName = parameter.Key,
					               Description = description,
					               Example = parameter.Value.Example,
					               Required = parameter.Value.Required
				               });
			}
			return properties;
		}

	    private string GetType(Parameter param)
	    {
	        if (param.Type == null)
                return "string";
	        
            if(NetTypeMapper.IsPrimitiveType(param.Type))
	            return NetTypeMapper.GetNetType(param.Type, param.Format) +
	                   (NetTypeMapper.GetNetType(param.Type, param.Format) == "string" || param.Required ? "" : "?");

	        var pureType = RamlTypesHelper.ExtractType(param.Type);

            if (schemaObjects.ContainsKey(pureType))
            {
                var apiObject = schemaObjects[pureType];
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }

	        return RamlTypesHelper.DecodeRaml1Type(param.Type);
	    }

    }
}