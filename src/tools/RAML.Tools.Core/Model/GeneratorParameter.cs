using System;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class GeneratorParameter
    {
        private readonly string[] reservedWords = { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal", "float", "double", "default", "is" };
        
        public string Type { get; set; }

        public string Description { get; set; }

        private string name;
        public string Name
        {
            get
            {
                if (reservedWords.Contains(name))
                    return "Ip" + name;

                return name; 
            }

            set {
                name = value;
                paramName = value;
            }
        }

        private string paramName;
        public string ParamName
        {
            get
            {
                if (reservedWords.Contains(paramName))
                    return "Ip" + paramName;

                return paramName;
            }

            set { paramName = value; }
        }

        public string OriginalName { get; set; }
    }
}