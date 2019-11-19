using System.Collections.Generic;
using System.Linq;
using RAML.Api.Core;
using RAML.Parser.Model;

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
            return new GeneratorParameter { OriginalName = parameter.Name,  Name = NetNamingMapper.Capitalize(NetNamingMapper.RemoveInvalidChars(parameter.Name)), Type = NewNetTypeMapper.GetNetType(parameter.Schema), Description = parameter.Description };
        }

    }
}