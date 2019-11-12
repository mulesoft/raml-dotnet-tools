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
                var objects = new Dictionary<string, ApiObject>();
                foreach(var obj in SchemaObjects)
                {
                    objects.Add(obj.Key, obj.Value);
                }
                foreach (var obj in RequestObjects)
                {
                    if(!objects.ContainsKey(obj.Key))
                        objects.Add(obj.Key, obj.Value);
                }
                foreach (var obj in ResponseObjects)
                {
                    if (!objects.ContainsKey(obj.Key))
                        objects.Add(obj.Key, obj.Value);
                }
                return objects.Values.ToArray();
            }
        }

        public string ModelsNamespace { get; set; }
        public IDictionary<string, ApiObject> SchemaObjects { get; set; }
        public IDictionary<string, ApiObject> ResponseObjects { get; set; }
        public IDictionary<string, ApiObject> RequestObjects { get; set; }
        public IEnumerable<ApiEnum> Enums { get; set; }
    }
}