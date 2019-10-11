using System.Collections.Generic;
using System.Linq;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public class ParametersMapper
    {
        public static IEnumerable<GeneratorParameter> Map(IDictionary<string, Parameter> parameters)
        {
            return parameters
                .Select(ConvertRAMLParameterToGeneratorParameter)
                .ToList();
        }

        private static GeneratorParameter ConvertRAMLParameterToGeneratorParameter(KeyValuePair<string, Parameter> parameter)
        {
            return new GeneratorParameter { Name = parameter.Key, Type = parameter.Value.Type, Description = parameter.Value.Description };
        }

    }
}