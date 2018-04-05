using AMF.Parser.Model;

namespace AMF.Common
{
    public class RamlInfo
    {
        public AmfModel RamlDocument { get; set; }
        public string RamlContents { get; set; }
        public string AbsolutePath { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasErrors { get { return !string.IsNullOrWhiteSpace(ErrorMessage); } }
    }
}