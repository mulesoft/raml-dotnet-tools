using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Raml.Common
{
	public class NetNamingMapper
	{
		private static readonly string[] reservedWords = {"Get", "Post", "Put", "Delete", "Options", "Head"};

		public static string GetNamespace(string title)
		{
			return NetNamingMapper.Capitalize(NetNamingMapper.RemoveIndalidChars(title));
		}

		public static string GetObjectName(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return "NullInput";

			var name = ReplaceSpecialChars(input, "-");
			name = ReplaceSpecialChars(name, "\\");
			name = ReplaceSpecialChars(name, "/");
			name = ReplaceSpecialChars(name, "_");
			name = ReplaceSpecialChars(name, ":");

			name = ReplaceSpecialChars(name, "{");
			name = ReplaceSpecialChars(name, "}");

			name = RemoveIndalidChars(name);

			if (reservedWords.Contains(name))
				name += "Object";

			if (StartsWithNumber(name))
				name = "O" + name;

			return name;
		}

		private static string ReplaceSpecialChars(string key, string separator)
		{
			return ReplaceSpecialChars(key, new[] {separator});
		}

		private static string ReplaceSpecialChars(string key, string[] separator)
		{
			var name = String.Empty;
			var words = key.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			return words.Aggregate(name, (current, word) => current + Capitalize(word));
		}

		public static string Capitalize(string word)
		{
			return word.Substring(0, 1).ToUpper() + word.Substring(1);
		}

		public static string RemoveIndalidChars(string input)
		{
			var validnamespace = Path.GetInvalidPathChars()
				.Aggregate(input, (current, invalidChar) => 
					current.Replace(invalidChar.ToString(CultureInfo.InvariantCulture), string.Empty));
			validnamespace = validnamespace.Replace(" ", string.Empty);
			validnamespace = validnamespace.Replace(".", string.Empty);
			return validnamespace;
		}

		public static string GetMethodName(string input)
		{
			var name = ReplaceSpecialChars(input, "-");
			name = ReplaceSpecialChars(name, "\\");
			name = ReplaceSpecialChars(name, "/");
			name = ReplaceSpecialChars(name, "_");
			name = ReplaceUriParameters(name);
			name = name.Replace(":", string.Empty);
			name = RemoveIndalidChars(name);

			if (StartsWithNumber(name))
				name = "M" + name;

			return name;
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

			input = input.Substring(0, input.IndexOf("{", StringComparison.InvariantCulture)) + "By" +
			        input.Substring(input.IndexOf("{", StringComparison.InvariantCulture));

			var name = String.Empty;
			var words = input.Split(new[] { "{", "}" }, StringSplitOptions.RemoveEmptyEntries);
			return words.Aggregate(name, (current, word) => current + Capitalize(word));
		}

		public static string GetPropertyName(string name)
		{
			var propName = name.Replace(":", string.Empty);
			propName = propName.Replace("/", string.Empty);
			propName = propName.Replace("-", string.Empty);
			propName = NetNamingMapper.Capitalize(propName);

			if (StartsWithNumber(propName))
				propName = "P" + propName;

			return propName;
		}
	}
}