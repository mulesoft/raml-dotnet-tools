using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Common
{
    public class RamlData
    {
        public RamlData(WebApiGeneratorModel model, string ramlContent, string filename)
        {
            Model = model;
            RamlContent = ramlContent;
            Filename = filename;
        }

        public WebApiGeneratorModel Model { get; private set; }
        public string RamlContent { get; private set; }
        public string Filename { get; private set; }
    }
}