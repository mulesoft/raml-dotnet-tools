using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using NuGet.VisualStudio;
using AMF.Common;
using AMF.Tools.Core;
using AMF.Tools.Core.WebApiGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using AMF.Api.Core;

namespace AMF.Tools
{
    public abstract class UnitTestsScaffoldServiceBase
    {
        private const string RamlSpecVersion = "1.0";
        private const string UnitTestsControllerTemplateName = "ApiControllerTests.t4";
        private const string UnitTestsControllerImplementationTemplateName = "ApiControllerTestsImplementation.t4";
        private const string ModelTemplateName = "ApiModel.t4";
        private const string EnumTemplateName = "ApiEnum.t4";

        private readonly TemplatesManager templatesManager = new TemplatesManager();
        private static readonly string ContractsFolder = Path.DirectorySeparatorChar + Settings.Default.ContractsFolderName + Path.DirectorySeparatorChar;
        private static readonly string IncludesFolder = Path.DirectorySeparatorChar + "includes" + Path.DirectorySeparatorChar;

        protected readonly string nugetPackagesSource = Settings.Default.NugetPackagesSource;
        
        private readonly CodeGenerator codeGenerator;

        protected readonly string UnitTestsFolderName = "Tests"; //TODO: get from user?
        protected readonly IServiceProvider ServiceProvider;

        public abstract void AddTests(RamlChooserActionParams parameters);

        public abstract string TemplateSubFolder { get; }

        protected abstract string GetTargetFolderPath(string testsFolderPath, string fileName);

        protected UnitTestsScaffoldServiceBase(IT4Service t4Service, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            codeGenerator = new CodeGenerator(t4Service);
        }

        public void Scaffold(string ramlSource, RamlChooserActionParams parameters)
        {
            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            ScaffoldToProject(parameters, proj);
        }

        public void ScaffoldToProject(RamlChooserActionParams parameters, Project proj)
        {
            var data = parameters.Data;
            if (data == null || data.RamlDocument == null)
                return;

            var model = new WebApiGeneratorService(data.RamlDocument, parameters.TargetNamespace).BuildModel();

            var unitTestsFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, UnitTestsFolderName);
            var unitTestsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar +
                                      UnitTestsFolderName + Path.DirectorySeparatorChar;

            var templates = new[]
            {
                UnitTestsControllerTemplateName,
                UnitTestsControllerImplementationTemplateName,
                ModelTemplateName,
                EnumTemplateName
            };
            if (!templatesManager.ConfirmWhenIncompatibleServerTemplate(unitTestsFolderPath, templates))
                return;

            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            AddOrUpdateUnitTestsControllerBase(parameters, unitTestsFolderPath, model, unitTestsFolderItem, extensionPath);
            AddOrUpdateUnitTestsControllerImplementations(parameters, unitTestsFolderPath, proj, model, unitTestsFolderItem, extensionPath);
        }

        private static void ScaffoldMainRamlFiles(IEnumerable<string> ramlFiles)
        {
            var service = GetScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);

            foreach (var ramlFile in ramlFiles)
            {
                var refFilePath = InstallerServices.GetRefFilePath(ramlFile);
                var includeApiVersionInRoutePrefix = RamlReferenceReader.GetRamlIncludeApiVersionInRoutePrefix(refFilePath);
                var parameters = new RamlChooserActionParams(ramlFile, ramlFile, null, null, Path.GetFileName(ramlFile),
                    RamlReferenceReader.GetRamlNamespace(refFilePath), null)
                {
                    UseAsyncMethods = RamlReferenceReader.GetRamlUseAsyncMethods(refFilePath),
                    IncludeApiVersionInRoutePrefix = includeApiVersionInRoutePrefix,
                    ModelsFolder = RamlReferenceReader.GetModelsFolder(refFilePath),
                    ImplementationControllersFolder = RamlReferenceReader.GetImplementationControllersFolder(refFilePath),
                    AddGeneratedSuffixToFiles = RamlReferenceReader.GetAddGeneratedSuffix(refFilePath)
                };
                service.Scaffold(ramlFile, parameters);
            }
        }

        public static UnitTestsScaffoldServiceBase GetScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider)
        {
            var dte = serviceProvider.GetService(typeof (SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            UnitTestsScaffoldServiceBase service;
            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                service = new UnitTestsScaffoldServiceAspNetCore(new T4Service(serviceProvider), serviceProvider);
            else
                service = new UnitTestsScaffoldServiceAspNetWebApi(new T4Service(serviceProvider), serviceProvider);
            return service;
        }

        private static IEnumerable<string> GetMainRamlFiles(Document document)
        {
            var path = document.Path.ToLowerInvariant();

            if (IsMainRamlFile(document, path))
                return new [] {document.FullName};

            var ramlItems = GetMainRamlFileFromProject();
            return GetItemsWithReferenceFiles(ramlItems);
        }

        private static bool IsMainRamlFile(Document document, string path)
        {
            return !path.EndsWith(IncludesFolder) && HasReferenceFile(document.FullName);
        }

        private static IEnumerable<string> GetItemsWithReferenceFiles(IEnumerable<ProjectItem> ramlItems)
        {
            return (from item in ramlItems where HasReferenceFile(item.FileNames[0]) select item.FileNames[0]).ToList();
        }

        private static bool HasReferenceFile(string ramlFilePath)
        {
            var refFilePath = InstallerServices.GetRefFilePath(ramlFilePath);
            var hasReferenceFile = !string.IsNullOrWhiteSpace(refFilePath) && File.Exists(refFilePath);
            return hasReferenceFile;
        }

        private static IEnumerable<ProjectItem> GetMainRamlFileFromProject()
        {
            var dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var contractsItem =
                proj.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == Settings.Default.ContractsFolderName);

            if (contractsItem == null)
                throw new InvalidOperationException("Could not find main file");

            var ramlItems = contractsItem.ProjectItems.Cast<ProjectItem>().Where(i => !i.Name.EndsWith(".ref")).ToArray();
            if (!ramlItems.Any())
                throw new InvalidOperationException("Could not find main file");

            return ramlItems;
        }

        private static bool IsInContractsFolder(Document document)
        {
            return document.Path.ToLowerInvariant().Contains(ContractsFolder.ToLowerInvariant());
        }

        private void AddOrUpdateUnitTestsControllerImplementations(RamlChooserActionParams parameters, string unitTestsFolderPath, Project proj,
            WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(unitTestsFolderPath, UnitTestsControllerImplementationTemplateName,
                Settings.Default.ControllerUnitTestsImplementationTemplateTitle, TemplateSubFolder);
            var unitTestsFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, UnitTestsFolderName);
            
            var templatesFolder = Path.Combine(unitTestsFolderPath, "Templates");
            var controllerImplementationTemplateParams =
                new TemplateParams<ControllerObject>(
                    Path.Combine(templatesFolder, UnitTestsControllerImplementationTemplateName),
                    unitTestsFolderItem, "controllerObject", model.Controllers, unitTestsFolderPath, folderItem,
                    extensionPath, parameters.TargetNamespace, "ControllerTestsImplementation", false,
                    GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
                {
                    TargetFolder = TargetFolderResolver.GetUnitTestsFolder(proj, UnitTestsFolderName),
                    RelativeFolder = UnitTestsFolderName,
                    Title = Settings.Default.ControllerUnitTestsImplementationTemplateTitle,
                    IncludeHasModels = true,
                    HasModels = model.Objects.Any(o => o.IsScalar == false) || model.Enums.Any(),
                    UseAsyncMethods = parameters.UseAsyncMethods,
                    IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                    ApiVersion = model.ApiVersion,
                    TestsNamespace = parameters.TestsNamespace
                };

            codeGenerator.GenerateCodeFromTemplate(controllerImplementationTemplateParams);
        }

        private static string GetVersionPrefix(bool includeApiVersionInRoutePrefix, string apiVersion)
        {
            return includeApiVersionInRoutePrefix ? NetNamingMapper.GetVersionName(apiVersion) : string.Empty;
        }

        private void AddOrUpdateUnitTestsControllerBase(RamlChooserActionParams parameters, string unitTestFolderPath, 
            WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(unitTestFolderPath, UnitTestsControllerTemplateName,
                Settings.Default.BaseControllerTestsTemplateTitle, TemplateSubFolder);
            var templatesFolder = Path.Combine(unitTestFolderPath, "Templates");

            var targetFolderPath = GetTargetFolderPath(unitTestFolderPath, Path.GetFileName(parameters.RamlFilePath));

            var controllerBaseTemplateParams =
                new TemplateParams<ControllerObject>(Path.Combine(templatesFolder, UnitTestsControllerTemplateName),
                    folderItem, "controllerObject", model.Controllers, targetFolderPath, folderItem, extensionPath,
                    parameters.TargetNamespace, "ControllerTests", true,
                    GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
                {
                    Title = Settings.Default.BaseControllerTestsTemplateTitle,
                    IncludeHasModels = true,
                    HasModels = model.Objects.Any(o => o.IsScalar == false) || model.Enums.Any(),
                    UseAsyncMethods = parameters.UseAsyncMethods,
                    IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                    ApiVersion = model.ApiVersion,
                    TargetFolder = targetFolderPath,
                    TestsNamespace = parameters.TestsNamespace
                };
            codeGenerator.GenerateCodeFromTemplate(controllerBaseTemplateParams);
        }

        protected void AddUnitTests(ProjectItem folderItem, string folderPath, RamlChooserActionParams parameters)
        {
            var includesFolderPath = folderPath + Path.DirectorySeparatorChar + InstallerServices.IncludesFolderName;

            var includesManager = new RamlIncludesManager();
            var result = includesManager.Manage(parameters.RamlSource, includesFolderPath, confirmOverrite: true, rootRamlPath: folderPath + Path.DirectorySeparatorChar);

            ManageIncludes(folderItem, result);

            var ramlProjItem = AddOrUpdateRamlFile(result.ModifiedContents, folderItem, folderPath, parameters.TargetFileName);
            InstallerServices.RemoveSubItemsAndAssociatedFiles(ramlProjItem);

            var targetFolderPath = GetTargetFolderPath(folderPath, parameters.TargetFileName);

            RamlProperties props = Map(parameters);
            var refFilePath = InstallerServices.AddRefFile(parameters.RamlFilePath, targetFolderPath, parameters.TargetFileName, props);
            ramlProjItem.ProjectItems.AddFromFile(refFilePath);

            Scaffold(ramlProjItem.FileNames[0], parameters);
        }

        protected abstract void ManageIncludes(ProjectItem folderItem, RamlIncludesManagerResult result);

        private RamlProperties Map(RamlChooserActionParams parameters)
        {
            return new RamlProperties
            {
                IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                UseAsyncMethods = parameters.UseAsyncMethods,
                Namespace = parameters.TargetNamespace,
                Source = parameters.RamlSource,
                ClientName = parameters.ClientRootClassName,
                ModelsFolder = parameters.ModelsFolder,
                ImplementationControllersFolder = parameters.ImplementationControllersFolder,
                AddGeneratedSuffix = parameters.AddGeneratedSuffixToFiles
            };
        }

        private static ProjectItem AddOrUpdateRamlFile(string modifiedContents, ProjectItem folderItem, string folderPath, string ramlFileName)
        {
            ProjectItem ramlProjItem;
            var ramlDestFile = Path.Combine(folderPath, ramlFileName);

            if (File.Exists(ramlDestFile))
            {
                var dialogResult = InstallerServices.ShowConfirmationDialog(ramlFileName);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    File.WriteAllText(ramlDestFile, modifiedContents);
                    ramlProjItem = folderItem.ProjectItems.AddFromFile(ramlDestFile);
                }
                else
                {
                    ramlProjItem = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == ramlFileName);
                    if (ramlProjItem == null)
                        ramlProjItem = folderItem.ProjectItems.AddFromFile(ramlDestFile);
                }
            }
            else
            {
                File.WriteAllText(ramlDestFile, modifiedContents);
                ramlProjItem = folderItem.ProjectItems.AddFromFile(ramlDestFile);
            }
            return ramlProjItem;
        }
    }
}