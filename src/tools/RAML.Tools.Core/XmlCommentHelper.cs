using System;
using System.Text;

namespace AMF.Tools.Core
{
    public static class XmlCommentHelper
    {
        public static string Escape(string text)
        {
            char unicodeNextLine = '\u0085';
            char unicodeLineSeparator = '\u2028';
            char unicodeParagraphSeparator = '\u2029';
            return text.Replace("\r\n", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Replace(unicodeNextLine.ToString(), string.Empty)
                .Replace(unicodeLineSeparator.ToString(), string.Empty)
                .Replace(unicodeParagraphSeparator.ToString(), string.Empty)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}