using System;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using MuleSoft.RAML.Tools.Properties;
using NuGet.VisualStudio;
using Raml.Common;

namespace MuleSoft.RAML.Tools
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

                AddFilesToProject(parameters.RamlFilePath, proj, parameters.TargetNamespace, parameters.RamlSource, parameters.TargetFileName, parameters.ClientRootClassName);
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

        protected override void GenerateCode(Project proj, string targetNamespace, string clientRootClassName, string apiRefsFolderPath,
            string ramlDestFile, string destFolderPath, string destFolderName, ProjectItem ramlProjItem)
        {
            ramlProjItem.Properties.Item("CustomTool").Value = string.Empty; // to cause a refresh when file already exists
            ramlProjItem.Properties.Item("CustomTool").Value = "RamlClientTool";
        }
    }
}