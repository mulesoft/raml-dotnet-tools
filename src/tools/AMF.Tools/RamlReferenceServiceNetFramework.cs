using System;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using NuGet.VisualStudio;
using AMF.Common;
using AMF.Tools.Core.ClientGenerator;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace AMF.Tools
{
    // Net 4.5 implementation
    public class RamlReferenceServiceNetFramework : RamlReferenceServiceBase 
    {

        private readonly string newtonsoftJsonPackageVersion = Settings.Default.NewtonsoftJsonPackageVersion;
        private readonly string webApiCorePackageId = Settings.Default.WebApiCorePackageId;
        private readonly string webApiCorePackageVersion = Settings.Default.WebApiCorePackageVersion;

        public RamlReferenceServiceNetFramework(IServiceProvider serviceProvider, ILogger logger) : base(serviceProvider, logger)
        {
        }

        public override void AddRamlReference(RamlChooserActionParams parameters)
        {
            try
            {
                Logger.LogInformation("Add RAML Reference process started");
                var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
                var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

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
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, NewtonsoftJsonPackageId, newtonsoftJsonPackageVersion, Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, webApiCorePackageId, webApiCorePackageVersion, Settings.Default.NugetExternalPackagesSource);
        }

        public override void GenerateCode(RamlInfo data, Project proj, string targetNamespace, string clientRootClassName, 
            string apiRefsFolderPath, string ramlDestFile, string destFolderPath, string destFolderName, ProjectItem ramlProjItem)
        {
            //ramlProjItem.Properties.Item("CustomTool").Value = string.Empty; // to cause a refresh when file already exists
            //ramlProjItem.Properties.Item("CustomTool").Value = "RamlClientTool";

            var model = new ClientGeneratorService(data.RamlDocument, clientRootClassName, targetNamespace).BuildModel();
            var directoryName = Path.GetDirectoryName(ramlDestFile).TrimEnd(Path.DirectorySeparatorChar);
            var templateFolder = directoryName.Substring(0, directoryName.LastIndexOf(Path.DirectorySeparatorChar)) +
                                 Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Client" + Path.DirectorySeparatorChar;

            var templateFilePath = Path.Combine(templateFolder, ClientT4TemplateName);
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            TemplatesManager.CopyClientTemplateToProjectFolder(apiRefsFolderPath, "Client");

            var t4Service = new T4Service(ServiceProvider);
            var res = t4Service.TransformText(templateFilePath, model, extensionPath, ramlDestFile, targetNamespace);

            if (res.HasErrors)
            {
                ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, res.Errors);
                MessageBox.Show(res.Errors);
            }
            else
            {
                var content = TemplatesManager.AddClientMetadataHeader(res.Content);
                var csTargetFile = Path.Combine(destFolderPath, destFolderName + ".cs");
                File.WriteAllText(csTargetFile, content);
                ramlProjItem.ProjectItems.AddFromFile(csTargetFile);
            }

        }
    }
}