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
    public class UnitTestsScaffoldServiceAspNetWebApi : UnitTestsScaffoldServiceBase
    {
        public UnitTestsScaffoldServiceAspNetWebApi(IT4Service t4Service, IServiceProvider serviceProvider): base(t4Service, serviceProvider){}

        public override string TemplateSubFolder
        {
            get { return "Tests"; }
        }

        public override void AddTests(RamlChooserActionParams parameters)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Tracking.Track("Unit Tests Scaffold (Asp.Net WebApi)");

            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, UnitTestsFolderName);
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + UnitTestsFolderName + Path.DirectorySeparatorChar;

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, parameters.TargetFileName);
            if (!Directory.Exists(targetFolderPath))
                Directory.CreateDirectory(targetFolderPath);

            InstallDependencies(proj);

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
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "MSTest.TestFramework", "1.3.2", Settings.Default.NugetExternalPackagesSource);

            //// Microsoft.AspNet.WebApi.Core
            var webApiPackage = "Microsoft.AspNet.WebApi.Core";
            var webApiVersion = "5.2.4";
            if (!installerServices.IsPackageInstalled(proj, webApiPackage))
            {
                installer.InstallPackage(nugetPackagesSource, proj, webApiPackage, webApiVersion, false);
            }

            //// AMF.Core.APICore
            //var ramlApiCorePackageId = Settings.Default.RAMLApiCorePackageId;
            //var ramlApiCorePackageVersion = Settings.Default.RAMLApiCorePackageVersion;
            //if (!installerServices.IsPackageInstalled(proj, ramlApiCorePackageId))
            //{
            //    installer.InstallPackage(nugetPackagesSource, proj, ramlApiCorePackageId, webApiVersion, false);
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