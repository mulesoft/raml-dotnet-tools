using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace AMF.Tools.Core
{
    [Serializable]
    public class Property : PropertyBase
    {
        public Property(string parentClassName = null) : base(parentClassName) { }

        public string Description { get; set; }

        private string type;
        public string Type 
        {
            get 
            {
                if (!Required && !type.EndsWith("?") && NewNetTypeMapper.IsPrimitiveType(type) && type != "string" && type != "object" && type != "byte[]")
                    return type + "?";

                return type;
            }
            set 
            {
                type = value;
            }
        }

        public string Example { get; set; }
        public bool Required { get; set; }
        public string StatusCode { get; set; }

        public string JSONSchema { get; set; }
        public bool IsEnum { get; set; }

        public bool IsAdditionalProperties => Name == "AdditionalProperties" && Type == "IDictionary<string, object>";

        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }

        public double? Maximum { get; set; }
        public double? Minimum { get; set; }

        public string Pattern { get; internal set; }


        public string CustomAttributes
        {
            get
            {
                var attributes = new Collection<string>();

                var identation = "".PadLeft(8);

                if (Required)
                    attributes.Add("[Required]".Insert(0, identation));

                if (MaxLength != null)
                    attributes.Add(string.Format("[MaxLength({0})]", MaxLength).Insert(0, identation));

                if (MinLength != null)
                    attributes.Add(string.Format("[MinLength({0})]", MinLength).Insert(0, identation));

                if (Minimum != null || Maximum != null)
                    BuildRangeAttribute(attributes, identation);

                if (!string.IsNullOrWhiteSpace(Pattern))
                {
                    Pattern = Pattern.Replace("\\\\", "\\");
                    if (IsValidEcmaRegex(Pattern))
                    {
                        if (Pattern.Contains("\""))
                            Pattern = Pattern.Replace("\"", "\"\"");

                        attributes.Add(string.Format("[RegularExpression(@\"{0}\")]", Pattern).Insert(0, identation));
                    }
                }

                if (!attributes.Any())
                    return string.Empty;

                return string.Join(Environment.NewLine, attributes);
            }
        }

        public Guid TypeId { get; internal set; }
        public string AmfId { get; set; }
        public string InheritanceProvenance { get; internal set; }

        private void BuildRangeAttribute(Collection<string> attributes, string identation)
        {
            if (Type == "int")
            {
                var attr = string.Format("[Range({0},{1:F0})]",
                    Minimum == null ? "int.MinValue" : Minimum.Value.ToString("F0"),
                    Maximum == null ? "int.MaxValue" : Maximum.Value.ToString("F0"));
                attributes.Add(attr.Insert(0, identation));
            }
            else
            {
                var enUs = new CultureInfo("en-US");
                var min = Minimum == null ? "double.MinValue" : Minimum.Value.ToString("F", enUs);
                var max = Maximum == null ? "double.MaxValue" : Maximum.Value.ToString("F", enUs);
                attributes.Add(string.Format("[Range({0},{1})]", min, max).Insert(0, identation));
            }
        }

        private static bool IsValidEcmaRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.Match("", pattern, RegexOptions.ECMAScript);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}