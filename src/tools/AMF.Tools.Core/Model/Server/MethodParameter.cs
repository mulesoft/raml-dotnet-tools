using System;

namespace AMF.Tools.Core.WebApiGenerator
{
    [Serializable]
    public class MethodParameter
    {
        public MethodParameter(bool isOptional, string parameterDeclaration)
        {
            IsOptional = isOptional;
            ParameterDeclaration = parameterDeclaration;
        }

        public bool IsOptional { get; set; }
        public string ParameterDeclaration { get; set; }
    }
}