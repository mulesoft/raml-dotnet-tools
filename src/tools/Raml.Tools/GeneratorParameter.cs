using System;
using System.Linq;

namespace Raml.Tools
{
    [Serializable]
    public class GeneratorParameter
    {
        private readonly string[] reservedWords = { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal", "float", "double" };
        private string name;

        public string Type { get; set; }

        public string Description { get; set; }

        public string Name
        {
            get
            {
                if (reservedWords.Contains(name))
                    return "Ip" + name;

                return name; 
            }

            set { name = value; }
        }
    }
}