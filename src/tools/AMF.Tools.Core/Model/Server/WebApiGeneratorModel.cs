using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core.WebApiGenerator
{
    [Serializable]
    public class WebApiGeneratorModel : ModelsGeneratorModel
    {
        private string baseUri;

        public IEnumerable<ControllerObject> Controllers { get; set; }

        public IEnumerable<GeneratorParameter> BaseUriParameters { get; set; }

        public string BaseUri
        {
            get { return !string.IsNullOrWhiteSpace(baseUri) && !baseUri.EndsWith("/") ? baseUri + "/" : baseUri; }
            set { baseUri = value; }
        }

        public Security Security { get; set; }

        public string BaseUriParametersString
        {
            get
            {
                if (BaseUriParameters == null || !BaseUriParameters.Any())
                    return string.Empty;

                return string.Join(",", BaseUriParameters
                    .Select(p => p.Type + " " + p.Name)
                    .ToArray());
            }
        }

        public string ApiVersion { get; set; }

        public string ControllersNamespace { get; set; }
    }
}