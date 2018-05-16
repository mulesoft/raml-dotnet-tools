//using System.Collections.Generic;
//using AMF.Parser.Model;

//namespace AMF.Tools.Core
//{
//    public class RamlTypeParser
//    {
//        private readonly IEnumerable<Shape> ramlTypes;
//        private readonly IDictionary<string, ApiObject> schemaObjects;
//        private readonly string targetNamespace;

//        private readonly IDictionary<string, string> warnings;
//        private readonly IDictionary<string, ApiEnum> enums;

//        public RamlTypeParser(IEnumerable<Shape> ramlTypes, IDictionary<string, ApiObject> schemaObjects, 
//            string targetNamespace, IDictionary<string, ApiEnum> enums, IDictionary<string, string> warnings)
//        {
//            this.ramlTypes = ramlTypes;
//            this.schemaObjects = schemaObjects;
//            this.targetNamespace = targetNamespace;
//            this.enums = enums;
//            this.warnings = warnings;
//        }

//        public void Parse()
//        {
//            foreach (var type in ramlTypes)
//            {
//                if (type == null)
//                    continue;

//                var apiObjects = ParseRamlType(type.Name, type, schemaObjects);
//                foreach (var apiObj in apiObjects)
//                {
//                    schemaObjects.Add(apiObj.Key, apiObj.Value);
//                }
//            }
//        }

//        public IDictionary<string, ApiObject> ParseInline(string key, Shape inline, IDictionary<string, ApiObject> objects)
//        {
//            return ParseRamlType(key, inline, objects);
//        }

//        private IDictionary<string, ApiObject> ParseRamlType(string key, Shape ramlType, IDictionary<string, ApiObject> schemaObjects)
//        {
//            return new ObjectParser().ParseObject(key, ramlType, schemaObjects, targetNamespace);
//        }
//    }
//}