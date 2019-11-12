using System.Collections.Generic;

namespace AMF.Tools
{
    public class TextFileHelper
    {
        public static int FindLineWith(IReadOnlyList<string> lines, string find)
        {
            var line = -1;
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains(find))
                    line = i;
            }
            return line;
        }
    }
}