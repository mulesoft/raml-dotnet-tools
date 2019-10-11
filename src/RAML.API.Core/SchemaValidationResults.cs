using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAML.Api.Core
{
    public class SchemaValidationResults
    {
        public bool IsValid { get; private set; }

        public IList<string> Errors { get; private set; }

        public SchemaValidationResults(bool isValid) : this(isValid, new List<string>())
        {
        }

        public SchemaValidationResults(bool isValid, IList<string> errors)
        {
            this.IsValid = isValid;
            this.Errors = errors;
        }
    }
}
