using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core
{
    [Serializable]
    public class PropertyBase
    {
        private readonly List<string> reservedWords = new List<string> { "ref", "out", "in", "base", "long", "int", "short", "bool", "string", "decimal",
            "float", "double", "null", "true", "false", "public", "private", "is" };

        private string name;

        public PropertyBase(string parentClassName = null)
        {
            if (!string.IsNullOrWhiteSpace(parentClassName))
                ParentClassName = parentClassName;
        }

        public string ParentClassName { get; internal set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SEC0115:Insecure Random Number Generator", Justification = "Not used in cryptographic nor any sensitive way. Only used to generate unique name")]
        public string Name
        {
            get
            {
                if (reservedWords.Contains(name.ToLowerInvariant()) || name == ParentClassName)
                    return "Ip" + name.ToLowerInvariant();

                var randInt = new Random().Next(short.MaxValue);
                if (string.IsNullOrWhiteSpace(name))
                    return "empty" + randInt;

                return name;
            }

            set { name = value; }
        }

        public string OriginalName { get; set; }
    }
}