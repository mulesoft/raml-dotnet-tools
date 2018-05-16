using System;

namespace AMF.Tools.Core
{
    public static class XmlCommentHelper
    {
        public static string Escape(string text)
        {
            return text.Replace("\r\n", string.Empty)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace(Environment.NewLine, string.Empty)
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }
    }
}