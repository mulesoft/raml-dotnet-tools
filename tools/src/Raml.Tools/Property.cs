using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Raml.Tools
{
    [Serializable]
    public class Property : PropertyBase
    {
        public Property(string parentClassName = null) : base(parentClassName) { }

        public string Description { get; set; }
        public string Type { get; set; }
        public string Example { get; set; }
        public bool Required { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public string JSONSchema { get; set; }
        public bool IsEnum { get; set; }

        public bool IsAdditionalProperties => Name == "AdditionalProperties" && Type == "IDictionary<string, object>";

        public int? MaxLength { get; set; }
        public int? MinLength { get; set; }

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
                
                if (!attributes.Any())
                    return string.Empty;

                return string.Join(Environment.NewLine, attributes);
            }
        }

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

        public double? Maximum { get; set; }
        public double? Minimum { get; set; }
    }
}