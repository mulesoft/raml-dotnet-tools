using System;
using System.Linq;

namespace Raml.Tools
{
    [Serializable]
    public class PropertyBase
    {
        private readonly string[] reservedWords = { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal", "float", "double" };
        private readonly string parentClassName;
        private string name;

        public PropertyBase(string parentClassName)
        {
            this.parentClassName = parentClassName;
        }

        public string Name
        {
            get
            {
                if (reservedWords.Contains(name.ToLowerInvariant()) || name == parentClassName)
                    return "Ip" + name.ToLowerInvariant();

                return name;
            }

            set { name = value; }
        }

        public string OriginalName { get; set; }
    }
}