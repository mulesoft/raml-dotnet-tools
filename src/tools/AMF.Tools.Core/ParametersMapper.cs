using System.Collections.Generic;
using System.Linq;
using AMF.Api.Core;
using AMF.Parser.Model;

namespace AMF.Tools.Core
{
    public class ParametersMapper
    {
        public static IEnumerable<GeneratorParameter> Map(IEnumerable<Parameter> parameters)
        {
            return parameters
                .Select(ConvertAmfParameterToGeneratorParameter)
                .ToList();
        }

        private static GeneratorParameter ConvertAmfParameterToGeneratorParameter(Parameter parameter)
        {
            return new GeneratorParameter { OriginalName = parameter.Name,  Name = NetNamingMapper.RemoveIndalidChars(parameter.Name), Type = NewNetTypeMapper.GetNetType(parameter.Schema), Description = parameter.Description };
        }

    }
}