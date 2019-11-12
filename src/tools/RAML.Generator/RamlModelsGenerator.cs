using System.IO;
using AMF.Parser.Model;
using AMF.Tools.Core.WebApiGenerator;
using System.Reflection;


namespace AMF.CLI
{
    public class RamlModelsGenerator : RamlBaseGenerator
    {
        private readonly AmfModel ramlDoc;
        private readonly string targetNamespace;

        public RamlModelsGenerator(AmfModel ramlDoc, string targetNamespace, string templatesFolder, 
            string targetFileName, string destinationFolder) : base(targetNamespace, templatesFolder, targetFileName, destinationFolder)
        {
            this.ramlDoc = ramlDoc;
            this.targetNamespace = targetNamespace;

            TemplatesFolder = string.IsNullOrWhiteSpace(templatesFolder)
                ? GetDefaultTemplateFolder()
                : templatesFolder;
        }

        private static string GetDefaultTemplateFolder()
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(typeof (Program)).Location) + Path.DirectorySeparatorChar +
                   "Templates" + Path.DirectorySeparatorChar + "WebApi2";
        }

        public void Generate()
        {
            var model = new ModelsGeneratorService(ramlDoc, targetNamespace).BuildModel();
            GenerateModels(model.Objects);
            GenerateEnums(model.Enums);
        }
    }
}