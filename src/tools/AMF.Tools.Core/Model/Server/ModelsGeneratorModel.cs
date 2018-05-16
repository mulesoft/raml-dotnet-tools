using System;
using System.Collections.Generic;
using System.Linq;

namespace AMF.Tools.Core.WebApiGenerator
{
    [Serializable]
    public class ModelsGeneratorModel
    {
        public ModelsGeneratorModel()
        {
            Warnings = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Warnings { get; set; }
        public virtual IEnumerable<ApiObject> Objects
        {
            get
            {
                var objects = SchemaObjects.Values.ToList();
                objects.AddRange(RequestObjects.Values);
                objects.AddRange(ResponseObjects.Values);
                return objects;
            }
        }

        public string Namespace { get; set; }
        public IDictionary<string, ApiObject> SchemaObjects { get; set; }
        public IDictionary<string, ApiObject> ResponseObjects { get; set; }
        public IDictionary<string, ApiObject> RequestObjects { get; set; }
        public IEnumerable<ApiEnum> Enums { get; set; }
    }
}