using System.Collections.Generic;
using System.Collections.ObjectModel;
using AMF.Api.Core;
using AMF.Parser.Model;

namespace AMF.Tools.Core.WebApiGenerator
{
    public class ModelsGeneratorService : GeneratorServiceBase
    {
        private readonly string modelsNamespace;

        public ModelsGeneratorService(AmfModel raml, string modelsNamespace) : base(raml)
        {
            this.modelsNamespace = modelsNamespace;
        }

        public ModelsGeneratorModel BuildModel()
        {
            classesNames = new Collection<string>();
            warnings = new Dictionary<string, string>();
            enums = new Dictionary<string, ApiEnum>();

            //new RamlTypeParser(raml.Shapes, schemaObjects, ns, enums, warnings).Parse();

            ParseSchemas();
            //schemaRequestObjects = GetRequestObjects();
            //schemaResponseObjects = GetResponseObjects();

            FixTypes(schemaObjects.Values);

            return new ModelsGeneratorModel
            {
                ModelsNamespace = modelsNamespace,
                SchemaObjects = schemaObjects,
                RequestObjects = schemaRequestObjects,
                ResponseObjects = schemaResponseObjects,
                Warnings = warnings,
                Enums = Enums
            };
        }

    }
}