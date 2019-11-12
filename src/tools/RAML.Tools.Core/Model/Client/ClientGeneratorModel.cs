using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core.ClientGenerator
{
    [Serializable]
    public class ClientGeneratorModel : Core.WebApiGenerator.ModelsGeneratorModel
    {
        private string baseUri;
        public IDictionary<string, ApiObject> QueryObjects { get; set; }
        public IDictionary<string, ApiObject> HeaderObjects { get; set; }
        public IDictionary<string, ApiObject> ResponseHeaderObjects { get; set; }
        public IEnumerable<ApiObject> ApiResponseObjects { get; set; }
        public IEnumerable<ApiObject> ApiRequestObjects { get; set; }
        public IDictionary<string, ApiObject> UriParameterObjects { get; set; }
        public IEnumerable<ClassObject> Classes { get; set; }

        public ClassObject Root { get; set; }

        private Dictionary<string, ApiObject> objects = null;
        public override IEnumerable<ApiObject> Objects
        {
            get
            {
                if (objects == null)
                    GetObjects();

                return objects.Values.ToArray();
            }
        }

        private void GetObjects()
        {
            objects = new Dictionary<string, ApiObject>();
            AddRange(objects, SchemaObjects);
            AddRange(objects, RequestObjects);
            AddRange(objects, ResponseObjects);
            AddRange(objects, QueryObjects);
            AddRange(objects, UriParameterObjects);
        }

        private void AddRange(IDictionary<string, ApiObject> objects, IDictionary<string, ApiObject> objectsToAdd)
        {
            foreach (var obj in objectsToAdd)
            {
                if (!objects.ContainsKey(obj.Key))
                    objects.Add(obj.Key, obj.Value);
            }
        }

        public IEnumerable<GeneratorParameter> BaseUriParameters { get; set; }

        public string BaseUri
        {
            get {
                if (string.IsNullOrWhiteSpace(baseUri))
                    return string.Empty;

                return !baseUri.EndsWith("/") ? baseUri + "/" : baseUri;
            }
            set { baseUri = value; }
        }

        public Security Security { get; set; }

        public string BaseUriParametersString
        {
            get
            {
                if (BaseUriParameters == null || !BaseUriParameters.Any())
                    return string.Empty;

                var baseUriParametersString = string.Join(",", BaseUriParameters
                    .Where(p => p.Name.ToLower() != "version")
                    .Select(p => p.Type + " " + p.Name)
                    .ToArray());

                if (BaseUriParameters.Any(p => p.Name.ToLower() == "version"))
                {
                    var versionParam = BaseUriParameters.First(p => p.Name.ToLower() == "version");
                    if (!string.IsNullOrWhiteSpace(Version))
                        baseUriParametersString += ", " + versionParam.Type + " " + versionParam.Name + " = \"" + Version + "\"";
                    else
                        baseUriParametersString += ", " + versionParam.Type + " " + versionParam.Name;
                }
                
                return baseUriParametersString;
            }
        }

        public string Version { get; set; }

        public string ConstructorParametersString
        {
            get
            {
                var res = "string baseUrl";
                if (string.IsNullOrWhiteSpace(BaseUriParametersString))
                    res += ", " + BaseUriParametersString;

                return res + ", HttpClient httpClient = null";
            }
        }

        public string BaseNamespace { get; internal set; }
    }
}