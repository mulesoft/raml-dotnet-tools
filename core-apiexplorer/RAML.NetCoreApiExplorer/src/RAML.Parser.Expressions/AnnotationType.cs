using System.Collections.Generic;

namespace Raml.Parser.Expressions
{
    public class AnnotationType
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public string Usage { get; set; }

        public IDictionary<string, Parameter> Parameters { get; set; }

        public bool AllowMultiple { get; set; }

        public ICollection<string> AllowedTargets { get; set; }
        public IDictionary<string, object> Annotations { get; set; }
    }
}