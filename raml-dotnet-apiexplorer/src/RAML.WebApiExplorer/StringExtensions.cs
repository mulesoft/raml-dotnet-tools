using System;

namespace RAML.WebApiExplorer
{
	public static class StringExtensions
	{
		public static string Indent(this string str, int quantity)
		{
			for (var i = 0; i < quantity; i++)
			{
				str = str.Insert(0, " ");
			}
			return str;
		}

        public static string IndentLines(this string str, int quantity)
        {
            var lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < lines.Length; j++)
            {
                var line = lines[j];
                for (var i = 0; i < quantity; i++)
                {
                    line = line.Insert(0, " ");
                }
                lines[j] = line;
            }
            return string.Join(Environment.NewLine, lines);
        }
    }
}