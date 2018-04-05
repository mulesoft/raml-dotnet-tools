using System.Collections.Generic;
using System.Collections.ObjectModel;
using Raml.Common;
using AMF.Parser.Model;

namespace AMF.Tools.Core.WebApiGenerator
{
    public class ModelsGeneratorService : GeneratorServiceBase
    {
        public ModelsGeneratorService(AmfModel raml, string targetNamespace) : base(raml, targetNamespace)
        {
        }

        public ModelsGeneratorModel BuildModel()
        {
            classesNames = new Collection<string>();
            warnings = new Dictionary<string, string>();
            enums = new Dictionary<string, ApiEnum>();

            var ns = string.IsNullOrWhiteSpace(raml.WebApi?.Name) ? targetNamespace : NetNamingMapper.GetNamespace(raml.WebApi.Name);

            //new RamlTypeParser(raml.Shapes, schemaObjects, ns, enums, warnings).Parse();

            ParseSchemas();
            //schemaRequestObjects = GetRequestObjects();
            //schemaResponseObjects = GetResponseObjects();

            return new ModelsGeneratorModel
            {
                SchemaObjects = schemaObjects,
                RequestObjects = schemaRequestObjects,
                ResponseObjects = schemaResponseObjects,
                Warnings = warnings,
                Enums = Enums
            };
        }

    }
}