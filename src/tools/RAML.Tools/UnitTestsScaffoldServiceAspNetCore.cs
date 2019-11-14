using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using AMF.Common;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft;

namespace AMF.Tools
{
    public class UnitTestsScaffoldServiceAspNetCore : UnitTestsScaffoldServiceBase
    {
        public UnitTestsScaffoldServiceAspNetCore(IT4Service t4Service, IServiceProvider serviceProvider): base(t4Service, serviceProvider){}

        public override string TemplateSubFolder
        {
            get { return "TestsCore"; }
        }

        public override void AddTests(RamlChooserActionParams parameters)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Tracking.Track("Unit Tests Scaffold (Asp.Net Core)");

            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            InstallDependencies(proj);

            var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, UnitTestsFolderName);
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + UnitTestsFolderName + Path.DirectorySeparatorChar;

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, parameters.TargetFileName);
            if (!Directory.Exists(targetFolderPath))
                Directory.CreateDirectory(targetFolderPath);

            AddUnitTests(folderItem, contractsFolderPath, parameters);
        }

        public override void InstallDependencies(Project proj)
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            Assumes.Present(componentModel);
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();
            // MSTests package
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "MSTest.TestFramework", "1.4.0", RAML.Tools.Properties.Settings.Default.NugetExternalPackagesSource);

            // InstallNugetDependencies(proj, newtonsoftJsonForCorePackageVersion);

            //// AMF.NetCore.APICore
            //var ramlNetCoreApiCorePackageId = RAML.Tools.Properties.Settings.Default.AMFNetCoreApiCorePackageId;
            //var ramlNetCoreApiCorePackageVersion = RAML.Tools.Properties.Settings.Default.AMFNetCoreApiCorePackageVersion;
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