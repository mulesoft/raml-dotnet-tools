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
    public abstract class RamlScaffoldServiceBase
    {
        private const string RamlSpecVersion = "1.0";
        private const string ControllerBaseTemplateName = "ApiControllerBase.t4";
        private const string ControllerInterfaceTemplateName = "ApiControllerInterface.t4";
        private const string ControllerImplementationTemplateName = "ApiControllerImplementation.t4";
        private const string ModelTemplateName = "ApiModel.t4";
        private const string EnumTemplateName = "ApiEnum.t4";

        private readonly TemplatesManager templatesManager = new TemplatesManager();
        private static readonly string ContractsFolder = Path.DirectorySeparatorChar + Settings.Default.ContractsFolderName + Path.DirectorySeparatorChar;
        private static readonly string IncludesFolder = Path.DirectorySeparatorChar + "includes" + Path.DirectorySeparatorChar;

        protected readonly string nugetPackagesSource = Settings.Default.NugetPackagesSource;
        private readonly string newtonsoftJsonPackageId = Settings.Default.NewtonsoftJsonPackageId;
        
        private readonly CodeGenerator codeGenerator;

        protected readonly string ContractsFolderName = Settings.Default.ContractsFolderName;
        protected readonly IServiceProvider ServiceProvider;

        public abstract void AddContract(RamlChooserActionParams parameters);

        public abstract string TemplateSubFolder { get; }

        protected abstract string GetTargetFolderPath(string contractsFolderPath, string fileName);

        protected RamlScaffoldServiceBase(IT4Service t4Service, IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            codeGenerator = new CodeGenerator(t4Service);
        }

        public void Scaffold(string ramlSource, RamlChooserActionParams parameters)
        {
            var data = parameters.Data;
            if (data == null || data.RamlDocument == null)
                return;

            var model = new WebApiGeneratorService(data.RamlDocument, parameters.TargetNamespace).BuildModel();

            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            var contractsFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, ContractsFolderName);
            var ramlItem =
                contractsFolderItem.ProjectItems.Cast<ProjectItem>()
                    .First(i => i.Name.ToLowerInvariant() == parameters.TargetFileName.ToLowerInvariant());
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar +
                                      ContractsFolderName + Path.DirectorySeparatorChar;

            var templates = new[]
            {
                ControllerBaseTemplateName, 
                ControllerInterfaceTemplateName, 
                ControllerImplementationTemplateName,
                ModelTemplateName, 
                EnumTemplateName
            };
            if (!templatesManager.ConfirmWhenIncompatibleServerTemplate(contractsFolderPath, templates))
                return;

            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            AddOrUpdateModels(parameters, contractsFolderPath, ramlItem, model, contractsFolderItem, extensionPath);

            AddOrUpdateEnums(parameters, contractsFolderPath, ramlItem, model, contractsFolderItem, extensionPath);

            AddOrUpdateControllerBase(parameters, contractsFolderPath, ramlItem, model, contractsFolderItem, extensionPath);

            AddOrUpdateControllerInterfaces(parameters, contractsFolderPath, ramlItem, model, contractsFolderItem, extensionPath);

            AddOrUpdateControllerImplementations(parameters, contractsFolderPath, proj, model, contractsFolderItem, extensionPath);

            AddJsonSchemaParsingErrors(model.Warnings, contractsFolderPath, contractsFolderItem, ramlItem);
        }

        private void AddJsonSchemaParsingErrors(IDictionary<string, string> warnings, string contractsFolderPath, 
            ProjectItem contractsFolderItem, ProjectItem ramlItem)
        {
            if(warnings.Count == 0)
                return;

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, ramlItem.FileNames[0]);

            JsonSchemaMessagesManager.AddJsonParsingErrors(warnings, contractsFolderItem, targetFolderPath);
        }

        public static void TriggerScaffoldOnRamlChanged(Document document)
        {
            if (!IsInContractsFolder(document)) 
                return;

             ScaffoldMainRamlFiles(GetMainRamlFiles(document));
        }

        protected void InstallNugetDependencies(Project proj, string packageVersion)
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();

            // AMF.Api.Core dependencies
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, newtonsoftJsonPackageId, packageVersion, Settings.Default.NugetExternalPackagesSource);

            // System.Xml.XmlSerializer 4.0.11-beta-23516
            // NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "System.Xml.XmlSerializer", "4.0.11-beta-23516");
        }

        private static void ScaffoldMainRamlFiles(IEnumerable<string> ramlFiles)
        {
            var service = GetRamlScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);

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

        public static RamlScaffoldServiceBase GetRamlScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider)
        {
            var dte = serviceProvider.GetService(typeof (SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            RamlScaffoldServiceBase service;
            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                service = new RamlScaffoldServiceAspNetCore(new T4Service(serviceProvider), serviceProvider);
            else
                service = new RamlScaffoldServiceWebApi(new T4Service(serviceProvider), serviceProvider);
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

        private void AddOrUpdateControllerImplementations(RamlChooserActionParams parameters, string contractsFolderPath, Project proj,
            WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(contractsFolderPath, ControllerImplementationTemplateName,
                Settings.Default.ControllerImplementationTemplateTitle, TemplateSubFolder);
            var controllersFolderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, "Controllers");
            
            var templatesFolder = Path.Combine(contractsFolderPath, "Templates");
            var controllerImplementationTemplateParams =
                new TemplateParams<ControllerObject>(
                    Path.Combine(templatesFolder, ControllerImplementationTemplateName),
                    controllersFolderItem, "controllerObject", model.Controllers, contractsFolderPath, folderItem,
                    extensionPath, parameters.TargetNamespace, "Controller", false,
                    GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
                {
                    TargetFolder = TargetFolderResolver.GetImplementationControllersFolderPath(proj, parameters.ImplementationControllersFolder),
                    RelativeFolder = parameters.ImplementationControllersFolder,
                    Title = Settings.Default.ControllerImplementationTemplateTitle,
                    IncludeHasModels = true,
                    HasModels = model.Objects.Any(o => o.IsScalar == false) || model.Enums.Any(),
                    UseAsyncMethods = parameters.UseAsyncMethods,
                    IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                    ApiVersion = model.ApiVersion
                };

            codeGenerator.GenerateCodeFromTemplate(controllerImplementationTemplateParams);
        }

        private static string GetVersionPrefix(bool includeApiVersionInRoutePrefix, string apiVersion)
        {
            return includeApiVersionInRoutePrefix ? NetNamingMapper.GetVersionName(apiVersion) : string.Empty;
        }

        private void AddOrUpdateControllerInterfaces(RamlChooserActionParams parameters, string contractsFolderPath, ProjectItem ramlItem,
            WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(contractsFolderPath, ControllerInterfaceTemplateName,
                Settings.Default.ControllerInterfaceTemplateTitle, TemplateSubFolder);
            var templatesFolder = Path.Combine(contractsFolderPath, "Templates");

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, ramlItem.FileNames[0]);

            var controllerInterfaceParams =
                new TemplateParams<ControllerObject>(Path.Combine(templatesFolder, ControllerInterfaceTemplateName),
                    ramlItem, "controllerObject", model.Controllers, targetFolderPath, folderItem, extensionPath,
                    parameters.TargetNamespace, "Controller", true,
                    "I" + GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
                {
                    Title = Settings.Default.ControllerInterfaceTemplateTitle,
                    IncludeHasModels = true,
                    HasModels = model.Objects.Any(o => o.IsScalar == false) || model.Enums.Any(),
                    UseAsyncMethods = parameters.UseAsyncMethods,
                    IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                    ApiVersion = model.ApiVersion,
                    TargetFolder = targetFolderPath
                };
            codeGenerator.GenerateCodeFromTemplate(controllerInterfaceParams);
        }

        private void AddOrUpdateControllerBase(RamlChooserActionParams parameters, string contractsFolderPath, ProjectItem ramlItem,
            WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(contractsFolderPath, ControllerBaseTemplateName,
                Settings.Default.BaseControllerTemplateTitle, TemplateSubFolder);
            var templatesFolder = Path.Combine(contractsFolderPath, "Templates");

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, ramlItem.FileNames[0]);

            var controllerBaseTemplateParams =
                new TemplateParams<ControllerObject>(Path.Combine(templatesFolder, ControllerBaseTemplateName),
                    ramlItem, "controllerObject", model.Controllers, targetFolderPath, folderItem, extensionPath,
                    parameters.TargetNamespace, "Controller", true,
                    GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
                {
                    Title = Settings.Default.BaseControllerTemplateTitle,
                    IncludeHasModels = true,
                    HasModels = model.Objects.Any(o => o.IsScalar == false) || model.Enums.Any(),
                    UseAsyncMethods = parameters.UseAsyncMethods,
                    IncludeApiVersionInRoutePrefix = parameters.IncludeApiVersionInRoutePrefix,
                    ApiVersion = model.ApiVersion,
                    TargetFolder = targetFolderPath
                };
            codeGenerator.GenerateCodeFromTemplate(controllerBaseTemplateParams);
        }

        private void AddOrUpdateModels(RamlChooserActionParams parameters, string contractsFolderPath, ProjectItem ramlItem, WebApiGeneratorModel model, ProjectItem contractsFolderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(contractsFolderPath, ModelTemplateName,
                Settings.Default.ModelsTemplateTitle, TemplateSubFolder);
            var templatesFolder = Path.Combine(contractsFolderPath, "Templates");
            
            var models = model.Objects;
            // when is an XML model, skip empty objects
            if (model.Objects.Any(o => !string.IsNullOrWhiteSpace(o.GeneratedCode)))
                models = model.Objects.Where(o => o.Properties.Any() || !string.IsNullOrWhiteSpace(o.GeneratedCode));

            // when array has no properties, set it collection on base type
            foreach(var arrayModel in models.Where(o => o.IsArray && o.Properties.Count == 0 && o.Type != null 
                            && CollectionTypeHelper.IsCollection(o.Type) && !NewNetTypeMapper.IsPrimitiveType(CollectionTypeHelper.GetBaseType(o.Type))))
            {
                arrayModel.BaseClass = arrayModel.Type.Substring(1); // remove the initil "I" to make it a concrete class
            }
            // skip array of primitives
            models = models.Where(o => o.Type == null || !(CollectionTypeHelper.IsCollection(o.Type) 
                                            && NewNetTypeMapper.IsPrimitiveType(CollectionTypeHelper.GetBaseType(o.Type))));
            models = models.Where(o => !o.IsScalar); // skip scalar types

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, ramlItem.FileNames[0]);

            var apiObjectTemplateParams = new TemplateParams<ApiObject>(
                Path.Combine(templatesFolder, ModelTemplateName), ramlItem, "apiObject", models,
                contractsFolderPath, contractsFolderItem, extensionPath, parameters.TargetNamespace,
                GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion) +
                (parameters.AddGeneratedSuffixToFiles ? ".generated" : string.Empty))
            {
                Title = Settings.Default.ModelsTemplateTitle,
                RelativeFolder = parameters.ModelsFolder,
                TargetFolder = TargetFolderResolver.GetModelsTargetFolder(ramlItem.ContainingProject,
                    targetFolderPath, parameters.ModelsFolder)
            };

            codeGenerator.GenerateCodeFromTemplate(apiObjectTemplateParams);
        }

        private void AddOrUpdateEnums(RamlChooserActionParams parameters, string contractsFolderPath, ProjectItem ramlItem, WebApiGeneratorModel model, ProjectItem folderItem, string extensionPath)
        {
            templatesManager.CopyServerTemplateToProjectFolder(contractsFolderPath, EnumTemplateName,
                Settings.Default.EnumsTemplateTitle, TemplateSubFolder);
            var templatesFolder = Path.Combine(contractsFolderPath, "Templates");

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, ramlItem.FileNames[0]);

            var apiEnumTemplateParams = new TemplateParams<ApiEnum>(
                Path.Combine(templatesFolder, EnumTemplateName), ramlItem, "apiEnum", model.Enums,
                targetFolderPath, folderItem, extensionPath, parameters.TargetNamespace,
                GetVersionPrefix(parameters.IncludeApiVersionInRoutePrefix, model.ApiVersion))
            {
                Title = Settings.Default.ModelsTemplateTitle,
                RelativeFolder = parameters.ModelsFolder,
                TargetFolder = TargetFolderResolver.GetModelsTargetFolder(ramlItem.ContainingProject,
                    targetFolderPath, parameters.ModelsFolder)
            };

            codeGenerator.GenerateCodeFromTemplate(apiEnumTemplateParams);
        }


        public void UpdateRaml(string ramlFilePath)
        {
            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + ContractsFolderName + Path.DirectorySeparatorChar;

            var refFilePath = InstallerServices.GetRefFilePath(ramlFilePath);
            var includesFolderPath = contractsFolderPath + Path.DirectorySeparatorChar + InstallerServices.IncludesFolderName;
            var ramlSource = RamlReferenceReader.GetRamlSource(refFilePath);
            if (string.IsNullOrWhiteSpace(ramlSource))
                ramlSource = ramlFilePath;

            var includesManager = new RamlIncludesManager();
            var result = includesManager.Manage(ramlSource, includesFolderPath, contractsFolderPath + Path.DirectorySeparatorChar);
            if (result.IsSuccess)
            {
                File.WriteAllText(ramlFilePath, result.ModifiedContents);
                var parameters = new RamlChooserActionParams(ramlFilePath, ramlFilePath, null, null,
                    Path.GetFileName(ramlFilePath).ToLowerInvariant(), 
                    RamlReferenceReader.GetRamlNamespace(refFilePath), null)
                {
                    UseAsyncMethods = RamlReferenceReader.GetRamlUseAsyncMethods(refFilePath),
                    IncludeApiVersionInRoutePrefix = RamlReferenceReader.GetRamlIncludeApiVersionInRoutePrefix(refFilePath),
                    ModelsFolder = RamlReferenceReader.GetModelsFolder(refFilePath),
                    ImplementationControllersFolder = RamlReferenceReader.GetImplementationControllersFolder(refFilePath),
                    AddGeneratedSuffixToFiles = RamlReferenceReader.GetAddGeneratedSuffix(refFilePath)
                };
                Scaffold(ramlFilePath, parameters);
            }
        }

        protected void AddContractFromFile(ProjectItem folderItem, string folderPath, RamlChooserActionParams parameters)
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

        protected void AddEmptyContract(ProjectItem folderItem, string folderPath, RamlChooserActionParams parameters)
        {
            
            var newContractFile = Path.Combine(folderPath, parameters.TargetFileName);
            var contents = CreateNewRamlContents(parameters.RamlTitle);

            ProjectItem ramlProjItem;
            if (File.Exists(newContractFile))
            {
                var dialogResult = InstallerServices.ShowConfirmationDialog(parameters.TargetFileName);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    File.WriteAllText(newContractFile, contents);
                    ramlProjItem = folderItem.ProjectItems.AddFromFile(newContractFile);
                }
                else
                {
                    ramlProjItem = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == newContractFile);
                    if (ramlProjItem == null)
                        ramlProjItem = folderItem.ProjectItems.AddFromFile(newContractFile);
                }
            }
            else
            {
                File.WriteAllText(newContractFile, contents);
                ramlProjItem = folderItem.ProjectItems.AddFromFile(newContractFile);
            }

            var props = Map(parameters);
            var targetFolderPath = GetTargetFolderPath(folderPath, parameters.TargetFileName);
            var refFilePath = InstallerServices.AddRefFile(newContractFile, targetFolderPath, parameters.TargetFileName, props);
            ramlProjItem.ProjectItems.AddFromFile(refFilePath);
        }



        private static string CreateNewRamlContents(string title)
        {
            var contents = "#%RAML " + RamlSpecVersion + Environment.NewLine +
                           "title: " + title + Environment.NewLine;
            return contents;
        }
    }
}