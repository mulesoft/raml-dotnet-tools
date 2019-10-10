using System.IO;
using Raml.Parser.Expressions;
using Raml.Tools.WebApiGenerator;
using System.Reflection;


namespace MuleSoft.RAMLGen
{
    public class RamlModelsGenerator : RamlBaseGenerator
    {
        private readonly RamlDocument ramlDoc;
        private readonly string targetNamespace;

        public RamlModelsGenerator(RamlDocument ramlDoc, string targetNamespace, string templatesFolder, 
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