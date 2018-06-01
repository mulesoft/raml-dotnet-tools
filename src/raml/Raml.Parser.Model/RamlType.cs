using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class RamlType
    {
        public RamlType()
        {
            Facets = new Dictionary<string, object>();
            OtherProperties = new Dictionary<string, object>();
            Examples = new List<string>();
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Example { get; set; }

        public ICollection<string> Examples { get; set; }

        public string DisplayName { get; set; }

        public string LibraryName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        // Can only be one of this 4
        public ExternalType External { get; set; }
        public ObjectType Object { get; set; }
        public ArrayType Array { get; set; }
        public Property Scalar { get; set; }

        public IDictionary<string, object> Facets { get; set; }
        public IDictionary<string, object> OtherProperties { get; set; }

    }
}