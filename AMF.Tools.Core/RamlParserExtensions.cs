using AMF.Parser;
using AMF.Parser.Model;

namespace AMF.Tools.Core
{
    public static class RamlParserExtensions
    {
        public static AmfModel Load(this AmfParser parser, string file)
        {
            //TODO: move to parser
            file = file.Substring(2).Replace("\\", "/");
            var task = parser.Load(file);
            task.Wait();
            return task.Result;
        }
    }
}