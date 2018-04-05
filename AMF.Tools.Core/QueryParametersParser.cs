using System.Collections.Generic;
using System.Linq;
using AMF.Parser.Model;
using AMF.Tools.Core.ClientGenerator;
using Raml.Common;

namespace AMF.Tools.Core
{
    public class QueryParametersParser
    {
        private readonly IDictionary<string, ApiObject> schemaObjects;

        public QueryParametersParser(IDictionary<string, ApiObject> schemaObjects)
        {
            this.schemaObjects = schemaObjects;
        }

        public ApiObject GetQueryObject(ClientGeneratorMethod generatedMethod, Operation method, string objectName)
        {
            var queryObject = new ApiObject { Name = generatedMethod.Name + objectName + "Query" };
            queryObject.Properties = ParseParameters(method);


            return queryObject;
        }

        public IList<Property> ParseParameters(Operation method)
        {
            return ConvertParametersToProperties(method.Request.QueryParameters);
        }

        public IList<Property> ConvertParametersToProperties(IEnumerable<Parameter> parameters)
        {
            var properties = new List<Property>();
            foreach (var parameter in parameters.Where(parameter => parameter != null && parameter.Schema != null))
            {
                var description = ParserHelpers.RemoveNewLines(parameter.Description);

				properties.Add(new Property
				               {
					               Type = NewNetTypeMapper.GetNetType(parameter.Schema),
					               Name = NetNamingMapper.GetPropertyName(parameter.Name),
                                   OriginalName = parameter.Name,
					               Description = description,
					               Example = ObjectParser.MapExample(parameter.Schema),
					               Required = parameter.Required
				               });
			}
			return properties;
		}

	    private string GetType(Parameter param) //TODO: check
	    {
	        if (param.Schema == null)
                return "string";
	        
            if(param.Schema is ScalarShape)
	            return NewNetTypeMapper.GetNetType((ScalarShape)param.Schema) +
	                   (NewNetTypeMapper.GetNetType((ScalarShape)param.Schema) == "string" || param.Required ? "" : "?");

            var pureType = param.Schema.Name;

            if (schemaObjects.ContainsKey(pureType))
            {
                var apiObject = schemaObjects[pureType];
                return RamlTypesHelper.GetTypeFromApiObject(apiObject);
            }

	        return pureType;
	    }

    }
}