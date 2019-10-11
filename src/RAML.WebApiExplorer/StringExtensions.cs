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
	}
}