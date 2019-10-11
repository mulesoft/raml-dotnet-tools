using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MuleSoft.RAML.Tools.Properties;
using NuGet.VisualStudio;
using Raml.Common;

namespace MuleSoft.RAML.Tools
{
    public abstract class RamlReferenceServiceBase
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger Logger;
        protected readonly string NewtonsoftJsonPackageId = Settings.Default.NewtonsoftJsonPackageId;

        private readonly string nugetPackagesSource = Settings.Default.NugetPackagesSource;

        private readonly string ramlApiCorePackageId = Settings.Default.RAMLApiCorePackageId;
        private readonly string ramlApiCorePackageVersion = Settings.Default.RAMLApiCorePackageVersion;
        public readonly static string ApiReferencesFolderName = Settings.Default.ApiReferencesFolderName;
        private readonly string microsoftNetHttpPackageId = Settings.Default.MicrosoftNetHttpPackageId;
        private readonly string microsoftNetHttpPackageVersion = Settings.Default.MicrosoftNetHttpPackageVersion;

        protected readonly string ClientT4TemplateName = Settings.Default.ClientT4TemplateName;
        protected readonly TemplatesManager TemplatesManager = new TemplatesManager();

        protected RamlReferenceServiceBase(IServiceProvider serviceProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            Logger = logger;
        }

        public static RamlReferenceServiceBase GetRamlReferenceService(ServiceProvider serviceProvider, ILogger logger)
        {
            var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            RamlReferenceServiceBase service;
            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                service = new RamlReferenceServiceNetCore(serviceProvider, logger);
            else
                service = new RamlReferenceServiceNetFramework(serviceProvider, logger);
            return service;            
        }

        public abstract void AddRamlReference(RamlChooserActionParams parameters);

        protected void InstallNugetDependencies(Project proj)
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, microsoftNetHttpPackageId, microsoftNetHttpPackageVersion, Settings.Default.NugetExternalPackagesSource);

            InstallNugetDependencies(proj, installer, packs);

            // RAML.Api.Core
            if (!installerServices.IsPackageInstalled(proj, ramlApiCorePackageId))
            {
                installer.InstallPackage(nugetPackagesSource, proj, ramlApiCorePackageId, ramlApiCorePackageVersion, false);
            }
        }

        protected abstract void InstallNugetDependencies(Project proj, IVsPackageInstaller installer, IVsPackageMetadata[] packs);

        protected void AddFilesToProject(string ramlSourceFile, Project proj, string targetNamespace, string ramlOriginalSource, string targetFileName, string clientRootClassName)
        {
            if(!File.Exists(ramlSourceFile))
                throw new FileNotFoundException("RAML file not found " + ramlSourceFile);

            if(Path.GetInvalidFileNameChars().Any(targetFileName.Contains))
                throw new ArgumentException("Specified filename has invalid chars: " + targetFileName);

            var destFolderName = Path.GetFileNameWithoutExtension(targetFileName);
            var apiRefsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + ApiReferencesFolderName + Path.DirectorySeparatorChar;
            var destFolderPath = apiRefsFolderPath + destFolderName + Path.DirectorySeparatorChar;
            var apiRefsFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, ApiReferencesFolderName);

            var destFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(apiRefsFolderItem, destFolderName, destFolderPath);

            var includesManager = new RamlIncludesManager();
            var result = includesManager.Manage(ramlOriginalSource, destFolderPath + Path.DirectorySeparatorChar + InstallerServices.IncludesFolderName, destFolderPath + Path.DirectorySeparatorChar);

            var ramlDestFile = Path.Combine(destFolderPath, targetFileName);
            if (File.Exists(ramlDestFile))
                new FileInfo(ramlDestFile).IsReadOnly = false;
            File.WriteAllText(ramlDestFile, result.ModifiedContents);

            var ramlProjItem = InstallerServices.AddOrUpdateRamlFile(ramlDestFile, destFolderPath, destFolderItem, targetFileName);
            var props = new RamlProperties
            {
                ClientName = clientRootClassName,
                Source = ramlOriginalSource,
                Namespace = targetNamespace
            };
            var refFilePath = InstallerServices.AddRefFile(ramlSourceFile, destFolderPath, targetFileName, props);
            ramlProjItem.ProjectItems.AddFromFile(refFilePath);

            GenerateCode(proj, targetNamespace, clientRootClassName, apiRefsFolderPath, ramlDestFile, destFolderPath, destFolderName, ramlProjItem);
        }

        protected abstract void GenerateCode(Project proj, string targetNamespace, string clientRootClassName, string apiRefsFolderPath,
            string ramlDestFile, string destFolderPath, string destFolderName, ProjectItem ramlProjItem);

    }
}