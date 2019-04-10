using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMF.Parser.Model;
using AMF.Tools.Core.WebApiGenerator;
using System.Reflection;


namespace AMF.CLI
{
    public class RamlServerGenerator : RamlBaseGenerator
    {
        private readonly string baseControllerT4Template = "ApiControllerBase.t4";
        private readonly string implementationControllerT4Template = "ApiControllerImplementation.t4";
        private readonly string interfaceControllerT4Template = "ApiControllerInterface.t4";

        private readonly AmfModel ramlDoc;
        private readonly string targetNamespace;
        private readonly string destinationFolder;
        private readonly bool useAsyncMethods;
        private bool hasModels;

        public RamlServerGenerator(AmfModel ramlDoc, string targetNamespace, string templatesFolder, 
            string targetFileName, string destinationFolder, bool useAsyncMethods, bool targetWebApi) 
            : base(targetNamespace, templatesFolder, targetFileName, destinationFolder)
        {
            this.ramlDoc = ramlDoc;
            this.targetNamespace = targetNamespace;
            this.destinationFolder = destinationFolder;
            this.useAsyncMethods = useAsyncMethods;

            TemplatesFolder = string.IsNullOrWhiteSpace(templatesFolder)
                ? GetDefaultTemplateFolder(targetWebApi)
                : templatesFolder;
        }

        private static string GetDefaultTemplateFolder(bool targetWebApi)
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location) + Path.DirectorySeparatorChar +
                   "Templates" + Path.DirectorySeparatorChar + (targetWebApi ? "WebApi2" : "AspNet5");
        }

        public void Generate()
        {
            var model = new WebApiGeneratorService(ramlDoc, targetNamespace, targetNamespace + ".Models").BuildModel();

            var models = model.Objects;
            // when is an XML model, skip empty objects
            if (models.Any(o => !string.IsNullOrWhiteSpace(o.GeneratedCode)))
                models = models.Where(o => o.Properties.Any() || !string.IsNullOrWhiteSpace(o.GeneratedCode));

            models = models.Where(o => !o.IsArray || o.Type == null); // skip array of primitives
            models = models.Where(o => !o.IsScalar); // skip scalars
            hasModels = models.Any();

            GenerateModels(model.Objects);
            GenerateEnums(model.Enums);
            GenerateInterfaceControllers(model.Controllers);
            GenerateBaseControllers(model.Controllers);
            GenerateImplementationControllers(model.Controllers);
        }

        private void GenerateImplementationControllers(IEnumerable<ControllerObject> controllers)
        {
            var extraParams = new Dictionary<string, bool>
            {
                {"useAsyncMethods", useAsyncMethods},
                {"hasModels", hasModels}
            };
            var destFolder = Path.Combine(destinationFolder, "Controllers").Replace("\\\\","\\") + Path.DirectorySeparatorChar;
            GenerateModels(controllers, "controllerObject", implementationControllerT4Template,
                o => o.Name + "Controller.cs", extraParams, destFolder);
        }

        private void GenerateBaseControllers(IEnumerable<ControllerObject> controllers)
        {
            var extraParams = new Dictionary<string, bool>
            {
                {"useAsyncMethods", useAsyncMethods},
                {"hasModels", hasModels}
            };
            GenerateModels(controllers, "controllerObject", baseControllerT4Template, o => o.Name + "Controller.cs", extraParams);
        }

        private void GenerateInterfaceControllers(IEnumerable<ControllerObject> controllers)
        {
            var extraParams = new Dictionary<string, bool>
            {
                {"useAsyncMethods", useAsyncMethods},
                {"hasModels", hasModels}
            };
            GenerateModels(controllers, "controllerObject", interfaceControllerT4Template, o => "I" + o.Name + "Controller.cs", extraParams);
        }
    }
}