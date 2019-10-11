using Raml.Parser;
using Raml.Parser.Expressions;

namespace Raml.Tools
{
    public static class RamlParserExtensions
    {
        public static RamlDocument Load(this RamlParser parser, string ramlFile)
        {
            var task = parser.LoadAsync(ramlFile);
            task.Wait();
            return task.Result;
        }
    }
}