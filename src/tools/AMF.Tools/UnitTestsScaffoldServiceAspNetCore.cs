using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using AMF.Common;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;

namespace AMF.Tools
{
    public class UnitTestsScaffoldServiceAspNetCore : UnitTestsScaffoldServiceBase
    {
        private readonly string newtonsoftJsonForCorePackageVersion = Settings.Default.NewtonsoftJsonForCorePackageVersion;

        public UnitTestsScaffoldServiceAspNetCore(IT4Service t4Service, IServiceProvider serviceProvider): base(t4Service, serviceProvider){}

        public override string TemplateSubFolder
        {
            get { return "Tests"; }
        }

        public override void AddTests(RamlChooserActionParams parameters)
        {
            Tracking.Track("Unit Tests Scaffold (Asp.Net Core)");

            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            InstallDependencies(proj, newtonsoftJsonForCorePackageVersion);

            var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, UnitTestsFolderName);
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + UnitTestsFolderName + Path.DirectorySeparatorChar;

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, parameters.TargetFileName);
            if (!Directory.Exists(targetFolderPath))
                Directory.CreateDirectory(targetFolderPath);

            AddUnitTests(folderItem, contractsFolderPath, parameters);
        }

        private void InstallDependencies(Project proj, string newtonsoftJsonForCorePackageVersion)
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();

            InstallNugetDependencies(proj, newtonsoftJsonForCorePackageVersion);

            //// AMF.NetCore.APICore
            //var ramlNetCoreApiCorePackageId = Settings.Default.AMFNetCoreApiCorePackageId;
            //var ramlNetCoreApiCorePackageVersion = Settings.Default.AMFNetCoreApiCorePackageVersion;
            //if (!installerServices.IsPackageInstalled(proj, ramlNetCoreApiCorePackageId))
            //{
            //    installer.InstallPackage(nugetPackagesSource, proj, ramlNetCoreApiCorePackageId, ramlNetCoreApiCorePackageVersion, false);
            //}
        }

        protected override string GetTargetFolderPath(string folderPath, string targetFilename)
        {
            return folderPath + Path.GetFileNameWithoutExtension(targetFilename) + Path.DirectorySeparatorChar;
        }

        protected override void ManageIncludes(ProjectItem folderItem, RamlIncludesManagerResult result)
        {
        }
    }
}