using System;
using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class Property : Parameter
    {
        public object Facets { get; set; }

        // number
        public string Format { get; set; }
        public int? MultipleOf { get; set; }

        // file
        public ICollection<string> FileTypes { get; set; }

        private string _libraryName;
        public new string LibraryName
        {
            get
            {
                if (_libraryName == null && !string.IsNullOrEmpty(Type) && Type.Contains("."))
                    _libraryName = Type.Remove(Type.LastIndexOf(".", StringComparison.Ordinal));
                return _libraryName;
            }
            set
            {
                _libraryName = value;
            }
        }
    }
}