using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class PropertyBase
    {
        private readonly List<string> reservedWords = new List<string> { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal", "float", "double" };
        private string name;

        public PropertyBase(string parentClassName = null)
        {
            if (!string.IsNullOrWhiteSpace(parentClassName))
                ParentClassName = parentClassName;
        }

        public string ParentClassName { get; internal set; }

        public string Name
        {
            get
            {
                if (reservedWords.Contains(name.ToLowerInvariant()) || name == ParentClassName)
                    return "Ip" + name.ToLowerInvariant();

                return name;
            }

            set { name = value; }
        }

        public string OriginalName { get; set; }
    }
}