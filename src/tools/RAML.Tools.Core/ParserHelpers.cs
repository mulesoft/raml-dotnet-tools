using System;
using System.Net;

namespace AMF.Tools.Core
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
    }
}
