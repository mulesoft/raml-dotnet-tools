using System;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class PropertyBase
    {
        private readonly string[] reservedWords = { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal", "float", "double" };
        private string name;



        public string Name
        {
            get
            {
                if (reservedWords.Contains(name.ToLowerInvariant()))
                    return "Ip" + name.ToLowerInvariant();

                return name;
            }

            set { name = value; }
        }

        public string OriginalName { get; set; }
    }
}