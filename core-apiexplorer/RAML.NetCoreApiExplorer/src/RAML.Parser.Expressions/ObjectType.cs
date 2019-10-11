using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class ObjectType
    {
        public ObjectType()
        {
            Properties = new Dictionary<string, RamlType>();
        }

        public int? MinProperties { get; set; }

        public int? MaxProperties { get; set; }

        public object AdditionalProperties { get; set; }

        public object PatternProperties { get; set; }

        public object Discriminator { get; set; }

        public string DiscriminatorValue { get; set; }

        public IDictionary<string, RamlType> Properties { get; set; }

    }
}