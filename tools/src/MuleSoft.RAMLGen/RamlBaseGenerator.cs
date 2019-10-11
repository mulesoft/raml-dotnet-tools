using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TextTemplating;
using Raml.Tools;
using Raml.Tools.WebApiGenerator;

namespace MuleSoft.RAMLGen
{
    public abstract class RamlBaseGenerator
    {
        private readonly string modelT4Template = "ApiModel.t4";
        private readonly string enumT4Template = "ApiEnum.t4";

        private readonly string targetNamespace;
        private readonly string targetFileName;
        private readonly string destinationFolder;
        private readonly CustomCmdLineHost host;
        private readonly Engine engine;

        protected string TemplatesFolder;

        protected RamlBaseGenerator(string targetNamespace, string templatesFolder, 
            string targetFileName, string destinationFolder)
        {
            TemplatesFolder = templatesFolder;
            this.targetNamespace = targetNamespace;
            this.targetFileName = targetFileName;
            this.destinationFolder = destinationFolder;

            host = new CustomCmdLineHost();
            engine = new Engine();
        }

        protected void GenerateEnums(IEnumerable<ApiEnum> enums)
        {
            GenerateModels(enums, "apiEnum", enumT4Template, o => o.Name + ".cs");
        }

        protected void GenerateModels(IEnumerable<ApiObject> models)
        {
            GenerateModels(models, "apiObject", modelT4Template, o => o.Name + ".cs");
        }

        protected void GenerateModels<T>(IEnumerable<T> models, string parameterKey, string t4TemplateName,
            Func<T, string> getName, IDictionary<string, bool> extraParams = null, string destFolder = null) where T : IHasName
        {
            foreach (var parameter in models)
            {
                var result = GenerateModel(parameter, parameterKey, t4TemplateName, extraParams);

                foreach (CompilerError error in result.Errors)
                {
                    Console.WriteLine(error.ToString());
                }

                if (destFolder == null)
                    destFolder = destinationFolder;

                if (!Directory.Exists(destFolder))
                    Directory.CreateDirectory(destFolder);

                if (result.Errors.Count == 0)
                    File.WriteAllText(Path.Combine(destFolder, getName(parameter)), result.Content, result.Encoding);
            }
        }

        private GenerationResult GenerateModel<T>(T parameter, string parameterKey, string t4TemplateName, IDictionary<string, bool> extraParams = null) where T : IHasName
        {
            host.TemplateFileValue = Path.Combine(TemplatesFolder, t4TemplateName);
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            // Read the T4 from disk into memory
            var templateFileContent = File.ReadAllText(Path.Combine(TemplatesFolder, t4TemplateName));
            templateFileContent = templateFileContent.Replace("$(binDir)", extensionPath);
            templateFileContent = templateFileContent.Replace("$(ramlFile)", targetFileName.Replace("\\", "\\\\"));
            templateFileContent = templateFileContent.Replace("$(namespace)", targetNamespace);

            host.Session = host.CreateSession();
            host.Session[parameterKey] = parameter;

            if (extraParams != null)
            {
                foreach (var extraParam in extraParams)
                {
                    host.Session[extraParam.Key] = extraParam.Value;
                }
            }

            var output = engine.ProcessTemplate(templateFileContent, host);

            return new GenerationResult
            {
                Content = output,
                Encoding = host.FileEncoding,
                Errors = host.Errors
            };
        }         
    }
}