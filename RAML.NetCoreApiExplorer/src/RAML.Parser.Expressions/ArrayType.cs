using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class ArrayType
    {
        public RamlType Items { get; set; }

        public bool? UniqueItems { get; set; }

        public int? MinItems { get; set; }

        public int? MaxItems { get; set; }
    }
}