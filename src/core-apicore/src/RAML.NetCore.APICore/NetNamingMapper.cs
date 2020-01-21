using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RAML.Common
{
	public class NetNamingMapper
	{
        private static readonly string[] ReservedWords = { "Get", "Post", "Put", "Delete", "Options", "Head", "ApiClient", "Type", "null", "true", "false", "is" };

		public static string GetNamespace(string title)
		{
			return Capitalize(RemoveInvalidChars(title));
		}

	    public static string GetVersionName(string input)
	    {
	        input = input.Replace(".", "_");
	        input = RemoveInvalidChars(input);
            input = input.Replace("+", string.Empty);
	        input = Capitalize(input);

	        if (StartsWithNumber(input))
	            input = "V" + input;

	        return input;
	    }

		public static string GetObjectName(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return "NullInput";

            var name = ReplaceSpecialChars(input, "{mediaTypeExtension}");
            name = ReplaceSpecialChars(name, "-");
			name = ReplaceSpecialChars(name, "\\");
			name = ReplaceSpecialChars(name, "/");
			name = ReplaceSpecialChars(name, "_");
			name = ReplaceSpecialChars(name, ":");
            name = ReplaceSpecialChars(name, "(");
            name = ReplaceSpecialChars(name, ")");
            name = ReplaceSpecialChars(name, "'");
            name = ReplaceSpecialChars(name, "`");
			name = ReplaceSpecialChars(name, "{");
			name = ReplaceSpecialChars(name, "}");
            name = ReplaceSpecialChars(name, "-");

            name = RemoveInvalidChars(name);

			if (ReservedWords.Contains(name))
				name += "Object";

			if (StartsWithNumber(name))
				name = "O" + name;

			return Capitalize(name);
		}

		private static string ReplaceSpecialChars(string key, string separator)
		{
			return ReplaceSpecialChars(key, new[] {separator});
		}

        private static string ReplaceSpecialChars(string key, string[] separator)
        {
            if (string.IsNullOrWhiteSpace(key))
                return key;

            var name = String.Empty;
            var words = key.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            var replaced = words.Aggregate(name, (current, word) => current + Capitalize(word));

            if (key[0].ToString() == replaced[0].ToString().ToLowerInvariant())
                return key.Substring(0, 1) + replaced.Substring(1);

            return replaced;
        }

        public static string Capitalize(string word)
		{
			return word.Substring(0, 1).ToUpper() + word.Substring(1);
		}

		public static string RemoveInvalidChars(string input)
		{
            var value = Path.GetInvalidPathChars()
                .Aggregate(input, (current, invalidChar) =>
                    current.Replace(invalidChar.ToString(), string.Empty));

			value = value.Replace(" ", string.Empty);
			value = value.Replace(".", string.Empty);
            value = value.Replace("?", string.Empty);
            value = value.Replace("[]", string.Empty);
            value = value.Replace("[", string.Empty);
            value = value.Replace("]", string.Empty);
            value = value.Replace("(", string.Empty);
            value = value.Replace(")", string.Empty);
            value = value.Replace("|", string.Empty);
            value = value.Replace("%", string.Empty);
            value = value.Replace("=", string.Empty);
            value = value.Replace("~", string.Empty);
            value = value.Replace("+", string.Empty);
            value = value.Replace(">=", "GreatOrEqual");
            value = value.Replace("<=", "LessOrEqual");
            value = value.Replace("<", "Less");
            value = value.Replace(">", "Great");
            value = value.Replace("*", "Asterisk");
            value = value.Replace("#", "Sharp");
            value = value.Replace("\\", "");
            value = ReplaceSpecialChars(value, "-");

            if (string.IsNullOrWhiteSpace(value))
            {
                var randInt = new Random(input.GetHashCode()).Next(short.MaxValue);
                return "A" + randInt;
            }

            return value.Trim();
		}

		public static bool HasIndalidChars(string input)
		{
            return (input.IndexOfAny(Path.GetInvalidPathChars()) >= 0);
		}

		public static string GetMethodName(string input)
		{
            var name = ReplaceSpecialChars(input, "{mediaTypeExtension}");
            name = ReplaceSpecialChars(name, "-");
			name = ReplaceSpecialChars(name, "\\");
			name = ReplaceSpecialChars(name, "/");
			name = ReplaceSpecialChars(name, "_");
            name = ReplaceSpecialChars(name, "(");
            name = ReplaceSpecialChars(name, ")");
            name = ReplaceSpecialChars(name, "'");
            name = ReplaceSpecialChars(name, "`");
			name = ReplaceUriParameters(name);
			name = name.Replace(":", string.Empty);
			name = RemoveInvalidChars(name);

			if (StartsWithNumber(name))
				name = "M" + name;

			return Capitalize(name);
		}

		private static bool StartsWithNumber(string name)
		{
			var startsWithNumber = new Regex("^[0-9]+");
			var nameStartsWithNumber = startsWithNumber.IsMatch(name);
			return nameStartsWithNumber;
		}

		private static string ReplaceUriParameters(string input )
		{
			if (!input.Contains("{"))
				return input;

            input = input.Substring(0, input.IndexOf("{", StringComparison.Ordinal)) + "By" +
                    input.Substring(input.IndexOf("{", StringComparison.Ordinal));

			var name = String.Empty;
			var words = input.Split(new[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries);
			return words.Aggregate(name, (current, word) => current + Capitalize(word));
		}

		public static string GetPropertyName(string name)
		{
			var propName = name.Replace(":", string.Empty);
			propName = propName.Replace("/", string.Empty);
			propName = propName.Replace("-", string.Empty);
            propName = propName.Replace("`", string.Empty);
		    propName = propName.Replace("?", string.Empty)
		        .Replace("[]", string.Empty)
		        .Replace("[", string.Empty)
		        .Replace("]", string.Empty)
		        .Replace("(", string.Empty)
		        .Replace(")", string.Empty)
                .Replace("|", string.Empty)
                .Replace("%", string.Empty)
                .Replace("~", string.Empty)
                .Replace("=", string.Empty);

            propName = propName.Replace("+", "Plus");
            propName = propName.Replace(".", "Dot");
            propName = RemoveInvalidChars(propName);
            propName = Capitalize(propName);

            if (ReservedWords.Contains(propName))
                propName = "A" + propName;

            if (StartsWithNumber(propName))
				propName = "P" + propName;

			return propName;
		}

	    public static string GetEnumValueName(string enumValue)
	    {
	        var value = enumValue
	            .Replace(":", string.Empty)
	            .Replace("/", string.Empty)
	            .Replace(" ", "_")
	            .Replace("-", "_")
                .Replace("~", string.Empty)
                .Replace("+", string.Empty)
	            .Replace(".", string.Empty);

            if (StartsWithNumber(value))
                value = "E" + value;

            value = RemoveInvalidChars(value);

            if (ReservedWords.Contains(value))
                value = "A" + value;

            if (int.TryParse(enumValue, out int number))
                value = value + " = " + number;

            return value;
	    }
	}
}