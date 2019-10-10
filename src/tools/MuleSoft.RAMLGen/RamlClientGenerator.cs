using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TextTemplating;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace MuleSoft.RAMLGen
{
    public class RamlClientGenerator
    {
        private static string ClientT4TemplateName = "RAMLCLient.t4";

        public void Generate(RamlDocument ramlDoc, string targetFileName, string targetNamespace, string templatesFolder,
            string destinationFolder)
        {
            templatesFolder = string.IsNullOrWhiteSpace(templatesFolder)
                ? GetTemplateDefaultPath()
                : templatesFolder;


            var model = new ClientGeneratorService(ramlDoc, targetFileName + "Client", targetNamespace).BuildModel();

            var templateFilePath = Path.Combine(templatesFolder, ClientT4TemplateName);
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            var host = new CustomCmdLineHost();
            var engine = new Engine();
            host.TemplateFileValue = templateFilePath;

            // Read the T4 from disk into memory
            var templateFileContent = File.ReadAllText(templateFilePath);
            templateFileContent = templateFileContent.Replace("$(binDir)", extensionPath);
            templateFileContent = templateFileContent.Replace("$(ramlFile)", targetFileName.Replace("\\", "\\\\"));
            templateFileContent = templateFileContent.Replace("$(namespace)", targetNamespace);

            host.Session = host.CreateSession();
            host.Session["model"] = model;

            var output = engine.ProcessTemplate(templateFileContent, host);

            foreach (CompilerError error in host.Errors)
            {
                Console.WriteLine(error.ToString());
            }

            //var t4Service = new T4Service(ServiceProvider.GlobalProvider);
            //var res = t4Service.TransformText(templateFilePath, model, extensionPath, opts.Source, targetNamespace);
            File.WriteAllText(Path.Combine(destinationFolder, targetFileName.Replace(".raml", ".cs")), output, host.FileEncoding);
        }

        private static string GetTemplateDefaultPath()
        {
            return Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location) + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Client";
        }
    }
}