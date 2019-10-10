using Raml.Parser.Expressions;

namespace Raml.Common
{
    public class RamlInfo
    {
        public RamlDocument RamlDocument { get; set; }
        public string RamlContents { get; set; }
        public string AbsolutePath { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasErrors { get { return !string.IsNullOrWhiteSpace(ErrorMessage); } }
    }
}