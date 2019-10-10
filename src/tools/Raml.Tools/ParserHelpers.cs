using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Raml.Tools
{
    public static class ParserHelpers
    {
        public static string RemoveNewLines(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            value = value.Replace("\r\n", " ");
            value = value.Replace("\r", " ");
            return value.Replace("\n", " ");
        }

        public static string GetStatusCode(string code)
        {
            HttpStatusCode statusCode;

            var description = Enum.TryParse(code, out statusCode) ? Enum.GetName(typeof (HttpStatusCode), Convert.ToInt32(code)) : null;

            return description ?? code;
        }
        public static HttpStatusCode GetHttpStatusCode(string code)
        {
            HttpStatusCode statusCode;

            Enum.TryParse(code, out statusCode);

            return statusCode;
        }
    }
}
