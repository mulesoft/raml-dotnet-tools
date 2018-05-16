using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using NuGet.VisualStudio;
using AMF.Common;
using AMF.Tools.Core.ClientGenerator;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace MuleSoft.RAML.Tools
{
    public class RamlReferenceServiceNetCore : RamlReferenceServiceBase
    {
        public RamlReferenceServiceNetCore(IServiceProvider serviceProvider, ILogger logger) : base(serviceProvider, logger)
        {
        }

        public override void AddRamlReference(RamlChooserActionParams parameters)
        {
            try
            {
                Logger.LogInformation("Add RAML Reference process started");
                var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
                var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

                AddPortableImports(proj);

                InstallNugetDependencies(proj);
                Logger.LogInformation("Nuget Dependencies installed");

                AddFilesToProject(parameters.Data, parameters.RamlFilePath, proj, parameters.TargetNamespace, parameters.RamlSource, parameters.TargetFileName, parameters.ClientRootClassName);
                Logger.LogInformation("Files added to project");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                var errorMessage = "Error when trying to add the RAML reference. " + ex.Message;
                if (ex.InnerException != null)
                    errorMessage += " - " + ex.InnerException;

                MessageBox.Show(errorMessage);
                throw;
            }
        }

        protected override void InstallNugetDependencies(Project proj, IVsPackageInstaller installer, IVsPackageMetadata[] packs)
        {
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, NewtonsoftJsonPackageId, "11.0.2", Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "Microsoft.AspNet.WebApi.Client", "5.2.3", Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "System.Xml.XmlSerializer", "4.3.0", Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "System.Runtime.Serialization.Xml", "4.3.0", Settings.Default.NugetExternalPackagesSource);
        }

        public override void GenerateCode(RamlInfo data, Project proj, string targetNamespace, string clientRootClassName, string apiRefsFolderPath,
            string ramlDestFile, string destFolderPath, string destFolderName, ProjectItem ramlProjItem)
        {
            TemplatesManager.CopyClientTemplateToProjectFolder(apiRefsFolderPath);
            GenerateCode(data, targetNamespace, clientRootClassName, ramlDestFile, destFolderPath, destFolderName);
        }

        public void GenerateCode(RamlInfo data, string targetNamespace, string clientRootClassName, string ramlDestFile, string destFolderPath,
            string destFolderName)
        {
            //var ramlInfo = await RamlInfoService.GetRamlInfo(ramlDestFile);
            //if (ramlInfo.HasErrors)
            //{
            //    ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, ramlInfo.ErrorMessage);
            //    MessageBox.Show(ramlInfo.ErrorMessage);
            //    return;
            //}

            var model = new ClientGeneratorService(data.RamlDocument, clientRootClassName, targetNamespace).BuildModel();
            var directoryName = Path.GetDirectoryName(ramlDestFile).TrimEnd(Path.DirectorySeparatorChar);
            var templateFolder = directoryName.Substring(0, directoryName.LastIndexOf(Path.DirectorySeparatorChar)) +
                                 Path.DirectorySeparatorChar + "Templates";

            var templateFilePath = Path.Combine(templateFolder, ClientT4TemplateName);
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;
            var t4Service = new T4Service(ServiceProvider);
            var res = t4Service.TransformText(templateFilePath, model, extensionPath, ramlDestFile, targetNamespace);

            if (res.HasErrors)
            {
                ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, res.Errors);
                MessageBox.Show(res.Errors);
                return;
            }

            var content = TemplatesManager.AddClientMetadataHeader(res.Content);
            var csTargetFile = Path.Combine(destFolderPath, destFolderName + ".cs");
            File.WriteAllText(csTargetFile, content);
        }

        private void AddPortableImports(Project proj)
        {
            var projectFile = GetProjectFilePath(proj);
            var lines = File.ReadAllLines(projectFile).ToList();
            if (!lines.Any(l => l.Contains("\"portable-net45+win8\"")))
            {
                if (lines.Any(l => l.Contains("\"imports\": \"dnxcore50\"")))
                {
                    var index = TextFileHelper.FindLineWith(lines, "\"imports\": \"dnxcore50\"");
                    lines.RemoveAt(index);
                    lines.Insert(index, "\"imports\": [\"dnxcore50\", \"portable-net45+win8\"]");
                }
                else if (lines.Any(l => l.Contains("\"dnxcore50\"")))
                {
                    var index = TextFileHelper.FindLineWith(lines, "\"dnxcore50\"");
                    lines[index] = lines[index].Replace("\"dnxcore50\"", "\"dnxcore50\", \"portable-net45+win8\"");
                }
                // dotnet5.6
                else if (lines.Any(l => l.Contains("\"imports\": \"dotnet5.6\"")))
                {
                    var index = TextFileHelper.FindLineWith(lines, "\"imports\": \"dotnet5.6\"");
                    lines.RemoveAt(index);
                    lines.Insert(index, "\"imports\": [\"dotnet5.6\", \"portable-net45+win8\"]");
                }
                else if (lines.Any(l => l.Contains("\"dotnet5.6\"")))
                {
                    var index = TextFileHelper.FindLineWith(lines, "\"dotnet5.6\"");
                    lines[index] = lines[index].Replace("\"dotnet5.6\"", "\"dotnet5.6\", \"portable-net45+win8\"");
                }

                File.WriteAllText(projectFile, string.Join(Environment.NewLine, lines));
            }
        }

        private static string GetProjectFilePath(Project proj)
        {
            return proj.FileName;
        }

    }
}