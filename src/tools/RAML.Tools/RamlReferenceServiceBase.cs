using System;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using AMF.Common;

namespace AMF.Tools
{
    public abstract class RamlReferenceServiceBase
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly ILogger Logger;
        protected readonly string NewtonsoftJsonPackageId = RAML.Tools.Properties.Settings.Default.NewtonsoftJsonPackageId;

        

        public readonly static string ApiReferencesFolderName = RAML.Tools.Properties.Settings.Default.ApiReferencesFolderName;

        protected readonly string ClientT4TemplateName = RAML.Tools.Properties.Settings.Default.ClientT4TemplateName;
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

        protected abstract void InstallNugetDependencies(Project proj);

        protected void AddFilesToProject(RamlInfo data, string ramlSourceFile, Project proj, string targetNamespace, string ramlOriginalSource, string targetFileName, string clientRootClassName)
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

            GenerateCode(data, proj, targetNamespace, clientRootClassName, apiRefsFolderPath, ramlDestFile, destFolderPath, destFolderName, ramlProjItem);
        }

        public abstract void GenerateCode(RamlInfo data, Project proj, string targetNamespace, string clientRootClassName, string apiRefsFolderPath,
            string ramlDestFile, string destFolderPath, string destFolderName, ProjectItem ramlProjItem);

    }
}