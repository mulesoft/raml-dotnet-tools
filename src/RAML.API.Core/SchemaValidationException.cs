using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAML.Api.Core
{
    public class SchemaValidationException : Exception
    {
        public IList<string> Errors { get; private set; }

        public SchemaValidationException(IList<string> errors) 
        {
            this.Errors = errors;
        }

        public SchemaValidationException(string message, IList<string> errors) : base(message) 
        {
            this.Errors = errors;
        }

        public SchemaValidationException(string message, Exception innerException, IList<string> errors) : base(message, innerException) 
        {
            this.Errors = errors;
        }
    }
}
