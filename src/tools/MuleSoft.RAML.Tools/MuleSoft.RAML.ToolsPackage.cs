//------------------------------------------------------------------------------
// <copyright file="MuleSoft_RAML_ToolsPackage.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System.Diagnostics.CodeAnalysis;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MuleSoft.RAML.Tools.Properties;
using NuGet.VisualStudio;
using Raml.Common;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Caliburn.Micro;
using Raml.Common.ViewModels;

namespace MuleSoft.RAML.Tools
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(MuleSoft_RAML_ToolsPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad("f1536ef8-92ec-443c-9ed7-fdadf150da82")]
    public sealed class MuleSoft_RAML_ToolsPackage : Package, IVsThreadedWaitDialogCallback
    {
        /// <summary>
        /// MuleSoft_RAML_ToolsPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "49d3aa6e-2e80-4568-92e9-4bcb3eb2b40d";

        private CommandID addReferenceInApiFolderCmdId;
        private CommandID updateReferenceCmdId;
        private CommandID updateRamlContractCommandId;
        private CommandID enableRamlMetadataOutputCommandId;
        private CommandID disableRamlMetadataOutputCommandId;
        private CommandID editRamlPropertiesCmdId;
        private CommandID extractRAMLCommandId;
        private static Events events;
        private static DocumentEvents documentEvents;
        private IVsThreadedWaitDialog3 attachingDialog;

        private IWindowManager windowManager;

        public MuleSoft_RAML_ToolsPackage()
        {
            var message = string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this);
            Debug.WriteLine(message);

#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, errors) => true;
#endif
        }

        protected override void Initialize()
        {
            var message = string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this);
            Debug.WriteLine(message);
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null == mcs) return;

            // Add RAML Reference command in References
            var addRamlRefCommandId = new CommandID(GuidList.guidMuleSoft_RAML_ReferencesNode, (int)PkgCmdIDList.cmdRAMLGenerator);
            var addRamlRefCommand = new OleMenuCommand(AddRamlReferenceCallback, addRamlRefCommandId);
            addRamlRefCommand.BeforeQueryStatus += AddRamlRefCommandOnBeforeQueryStatus;
            mcs.AddCommand(addRamlRefCommand);

            // Add RAML Reference command in Api References Folder
            addReferenceInApiFolderCmdId = new CommandID(GuidList.guidMuleSoft_RAML_FolderNode, (int)PkgCmdIDList.cmdRAMLGenerator2);
            var addReferenceCommand = new OleMenuCommand(AddRamlReferenceCallback, addReferenceInApiFolderCmdId);
            addReferenceCommand.BeforeQueryStatus += AddRamlRefCommand_BeforeQueryStatus;
            mcs.AddCommand(addReferenceCommand);

            // Update RAML Reference command
            updateReferenceCmdId = new CommandID(GuidList.guidMuleSoft_RAML_FileNode, (int)PkgCmdIDList.cmdUpdateRAMLReference);
            var updateReferenceCommand = new OleMenuCommand(UpdateRamlRefCallback, updateReferenceCmdId);
            updateReferenceCommand.BeforeQueryStatus += UpdateRamlRefCommand_BeforeQueryStatus;
            mcs.AddCommand(updateReferenceCommand);

            // Add RAML Contract command in Project Node
            var addRamlContractCmdId = new CommandID(GuidList.guidMuleSoft_RAML_ProjectNode, (int)PkgCmdIDList.cmdAddContract);
            var addRamlContractCommand = new OleMenuCommand(AddRamlContractCallback, addRamlContractCmdId);
            addRamlContractCommand.BeforeQueryStatus += AddRamlContractCommandOnBeforeQueryStatus;
            mcs.AddCommand(addRamlContractCommand);

            // Add RAML Contract command in Folder Node
            var addRamlContractFolderCmdId = new CommandID(GuidList.guidMuleSoft_RAML_ContractFolderNode, (int)PkgCmdIDList.cmdAddContract2);
            var addRamlContractFolderCommand = new OleMenuCommand(AddRamlContractCallback, addRamlContractFolderCmdId);
            addRamlContractFolderCommand.BeforeQueryStatus += AddRamlContractFolderCommandOnBeforeQueryStatus;
            mcs.AddCommand(addRamlContractFolderCommand);

            // Update RAML from source (Contract/Server) command
            updateRamlContractCommandId = new CommandID(GuidList.guidMuleSoft_RAML_CmdUpdateRAMLContract, (int)PkgCmdIDList.cmdUpdateRAMLContract);
            var updateRamlCommand = new OleMenuCommand(UpdateRamlContractCallback, updateRamlContractCommandId);
            updateRamlCommand.BeforeQueryStatus += UpdateRAMLCommandOnBeforeQueryStatus;
            mcs.AddCommand(updateRamlCommand);

            // Enable RAML metadata output (RAML WebApiExplorer) command
            enableRamlMetadataOutputCommandId = new CommandID(GuidList.guidMuleSoft_RAML_EnableMetadataOutput, (int)PkgCmdIDList.cmdEnableMetadataOutput);
            var enableRamlMetadataOutput = new OleMenuCommand(EnableRamlMetadataOutputCallback, enableRamlMetadataOutputCommandId);
            enableRamlMetadataOutput.BeforeQueryStatus += AddReverseEngineeringCommandOnBeforeQueryStatus;
            mcs.AddCommand(enableRamlMetadataOutput);

            // Disable RAML metadata output (RAML WebApiExplorer) command
            disableRamlMetadataOutputCommandId = new CommandID(GuidList.guidMuleSoft_RAML_DisableMetadataOutput, (int)PkgCmdIDList.cmdDisableMetadataOutput);
            var disableRamlMetadataOutput = new OleMenuCommand(DisableRamlMetadataOutputCallback, disableRamlMetadataOutputCommandId);
            disableRamlMetadataOutput.BeforeQueryStatus += RemoveReverseEngineeringCommandOnBeforeQueryStatus;
            mcs.AddCommand(disableRamlMetadataOutput);

            // Edit RAML Properties command
            editRamlPropertiesCmdId = new CommandID(GuidList.guidMuleSoft_RAML_EditProperties, (int)PkgCmdIDList.cmdEditRamlProperties);
            var editRamlPropertiesCommand = new OleMenuCommand(EditRamlPropertiesCallback, editRamlPropertiesCmdId);
            editRamlPropertiesCommand.BeforeQueryStatus += EditRamlPropertiesCommand_BeforeQueryStatus;
            mcs.AddCommand(editRamlPropertiesCommand);

            //// Extract RAML (RAML WebApiExplorer) command
            extractRAMLCommandId = new CommandID(GuidList.guidMuleSoft_RAML_ExtractRAML, (int)PkgCmdIDList.cmdExtractRAML);
            var extractRAMLCommand = new OleMenuCommand(ExtractRAMLCallback, extractRAMLCommandId);
            extractRAMLCommand.BeforeQueryStatus += ExtractRAMLCommandOnBeforeQueryStatus;
            mcs.AddCommand(extractRAMLCommand);

            // trigger scaffold when RAML document gets saved
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            events = dte.Events;
            documentEvents = events.DocumentEvents;
            documentEvents.DocumentSaved += DocumentEventsOnDocumentSaved;
            //MuleSoft.RAML.Tools.Command1.Initialize(this);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize();

            try
            {
                windowManager = IoC.Get<IWindowManager>();
            }
            catch
            {
                windowManager = new WindowManager();
            }

            LoadSystemWindowsInteractivity();
        }

        private void DocumentEventsOnDocumentSaved(Document document)
        {
            RamlScaffoldServiceBase.TriggerScaffoldOnRamlChanged(document);

            RamlClientTool.TriggerClientRegeneration(document, GetExtensionPath());
        }

        // workaround http://stackoverflow.com/questions/29362125/visual-studio-extension-could-not-find-a-required-assembly
        private static void LoadSystemWindowsInteractivity()
        {
            // HACK: Force load System.Windows.Interactivity.dll from plugin's 
            // directory
            typeof(System.Windows.Interactivity.Behavior).ToString();
        }



        private void AddRamlRefCommandOnBeforeQueryStatus(object sender, EventArgs eventArgs)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private void AddRamlContractFolderCommandOnBeforeQueryStatus(object sender, EventArgs eventArgs)
        {
            ShowOrHideCommandAddContractFolder(sender);
        }

        private void DisableRamlMetadataOutputCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(enableRamlMetadataOutputCommandId, false);

            StartProgressBar("Disable RAML metadata output", "Uninstalling...", "Processing...");

            var service = ReverseEngineeringServiceBase.GetReverseEngineeringService(ServiceProvider.GlobalProvider);
            service.RemoveReverseEngineering();

            StopProgressBar();

            ChangeCommandStatus(enableRamlMetadataOutputCommandId, true);
        }

        private void ExtractRAMLCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(extractRAMLCommandId, false);

            var service = ReverseEngineeringServiceBase.GetReverseEngineeringService(ServiceProvider.GlobalProvider);
            service.ExtractRAML();

            ChangeCommandStatus(extractRAMLCommandId, true);
        }

        private void EnableRamlMetadataOutputCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(enableRamlMetadataOutputCommandId, false);

            StartProgressBar("Enable RAML metadata output", "Installing...", "Processing...");

            var service = ReverseEngineeringServiceBase.GetReverseEngineeringService(ServiceProvider.GlobalProvider);
            service.AddReverseEngineering();

            StopProgressBar();

            System.Diagnostics.Process.Start("https://github.com/mulesoft-labs/raml-dotnet-tools#metadata-extract-a-raml-definition-from-your-web-app");
            ChangeCommandStatus(enableRamlMetadataOutputCommandId, true);
        }

        private void UpdateRamlContractCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(updateRamlContractCommandId, false);

            // Get the file path
            uint itemid;
            IVsHierarchy hierarchy;
            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            string ramlFilePath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out ramlFilePath);

            var ramlScaffoldUpdater = RamlScaffoldServiceBase.GetRamlScaffoldService(ServiceProvider.GlobalProvider);
            ramlScaffoldUpdater.UpdateRaml(ramlFilePath);

            ChangeCommandStatus(updateRamlContractCommandId, true);
        }

        private void AddRamlContractCallback(object sender, EventArgs e)
        {
            var ramlScaffoldUpdater = RamlScaffoldServiceBase.GetRamlScaffoldService(ServiceProvider.GlobalProvider);
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(ServiceProvider.GlobalProvider, ramlScaffoldUpdater.AddContract, "Add RAML Contract", true, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 570;
            windowManager.ShowDialog(ramlChooserViewModel, null, settings);
            //var frm = new RamlChooserView(ServiceProvider.GlobalProvider, ramlScaffoldUpdater.AddContract, "Add RAML Contract", true, Settings.Default.RAMLExchangeUrl);
            //frm.ShowDialog();
        }

        private void AddRamlReferenceCallback(object sender, EventArgs e)
        {
            var generationServices = RamlReferenceServiceBase.GetRamlReferenceService(ServiceProvider.GlobalProvider, new ActivityLogger());
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(this, generationServices.AddRamlReference, "Add RAML Reference", false, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 475;
            windowManager.ShowDialog(ramlChooserViewModel, null, settings);
            //var ramlChooser = new RamlChooserView(this, generationServices.AddRamlReference, "Add RAML Reference", false, Settings.Default.RAMLExchangeUrl);
            //ramlChooser.ShowDialog();
        }

        private void UpdateRamlRefCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(updateReferenceCmdId, false);

            // Get the file path
            uint itemid;
            IVsHierarchy hierarchy;
            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            string ramlFilePath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out ramlFilePath);

            var templatesManager = new TemplatesManager();
            var ramlFolder = Path.GetDirectoryName(ramlFilePath).TrimEnd(Path.DirectorySeparatorChar);
            var generatedFolderPath = ramlFolder.Substring(0, ramlFolder.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            if (!templatesManager.ConfirmWhenIncompatibleClientTemplate(generatedFolderPath))
                return;

            RegenerateClientCode(ramlFilePath);
            
            ChangeCommandStatus(updateReferenceCmdId, true);
        }

        private void RegenerateClientCode(string ramlFilePath)
        {
            if (IsAJsonOrXProj())
            {
                var result = RamlClientTool.RegenerateCode(ramlFilePath, GetExtensionPath());
                if (!result.IsSuccess)
                {
                    ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, result.ErrorMessage);
                    MessageBox.Show(result.ErrorMessage);
                }
            }
            else
            {
                var dte = (DTE2) GetService(typeof (SDTE));
                dte.ExecuteCommand("Project.RunCustomTool");
            }
        }

        private string GetExtensionPath()
        {
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;
            return extensionPath;
        }

        private void EditRamlPropertiesCallback(object sender, EventArgs e)
        {
            ChangeCommandStatus(editRamlPropertiesCmdId, false);

            // Get the file path
            uint itemid;
            IVsHierarchy hierarchy;
            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            string ramlFilePath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out ramlFilePath);

            var refFilePath = InstallerServices.GetRefFilePath(ramlFilePath);


            var editorModel = new RamlPropertiesEditorViewModel();
            editorModel.Load(refFilePath, Settings.Default.ContractsFolderName, Settings.Default.ApiReferencesFolderName);
            windowManager.ShowDialog(editorModel);

            //var frm = new RamlPropertiesEditor();
            //frm.Load(refFilePath, Settings.Default.ContractsFolderName, Settings.Default.ApiReferencesFolderName);
            //var result = frm.ShowDialog();
            if (editorModel.WasSaved)
            {

                if (IsServerSide(ramlFilePath))
                {
                    var ramlScaffoldUpdater = RamlScaffoldServiceBase.GetRamlScaffoldService(ServiceProvider.GlobalProvider);
                    ramlScaffoldUpdater.UpdateRaml(ramlFilePath);
                }
                else
                {
                    var templatesManager = new TemplatesManager();
                    var ramlFolder = Path.GetDirectoryName(ramlFilePath).TrimEnd(Path.DirectorySeparatorChar);
                    var generatedFolderPath = ramlFolder.Substring(0, ramlFolder.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                    if (!templatesManager.ConfirmWhenIncompatibleClientTemplate(generatedFolderPath))
                        return;

                    RegenerateClientCode(ramlFilePath);
                }
            }

            ChangeCommandStatus(editRamlPropertiesCmdId, true);
        }

        private bool IsServerSide(string ramlFilePath)
        {
            if (ramlFilePath.Contains(Settings.Default.ContractsFolderName) && !ramlFilePath.Contains(Settings.Default.ApiReferencesFolderName))
                return true;

            if (!ramlFilePath.Contains(Settings.Default.ContractsFolderName) && ramlFilePath.Contains(Settings.Default.ApiReferencesFolderName))
                return false;

            throw new InvalidOperationException("Cannot determine if the raml is used on the server or the client");
        }


        //private bool Unauthorized(string ramlFilePath)
        //{
        //    var refFilePath = InstallerServices.GetRefFilePath(ramlFilePath);
        //    var ramlSource = RamlReferenceReader.GetRamlSource(refFilePath);
        //    if (!ramlSource.StartsWith("http"))
        //        return false;

        //    var client = new HttpClient();
        //    var task = client.SendAsync(new HttpRequestMessage(HttpMethod.Get, ramlSource));
        //    task.WaitWithPumping();
        //    var result = task.ConfigureAwait(false).GetAwaiter().GetResult();
        //    return result.StatusCode == HttpStatusCode.Unauthorized;
        //}

        private void AddRamlRefCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            ShowOrHideCommandAddRefApiFolder(sender);
        }

        private void UpdateRAMLCommandOnBeforeQueryStatus(object sender, EventArgs e)
        {
            ShowOrHideUpdateRamlCommandContract(sender);
        }

        private void ExtractRAMLCommandOnBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (!VisualStudioAutomationHelper.IsANetCoreProject(proj) && !IsWebApiCoreInstalled(proj))
                return;

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && !IsAspNet5MvcInstalled(proj))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private void AddReverseEngineeringCommandOnBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (!VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!IsWebApiCoreInstalled(proj) || IsWebApiExplorerInstalled()))
                return;

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!IsAspNet5MvcInstalled(proj) || IsNetCoreApiExplorerInstalled()))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private void RemoveReverseEngineeringCommandOnBeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && !IsNetCoreApiExplorerInstalled())
                return;

            if (!VisualStudioAutomationHelper.IsANetCoreProject(proj) && !IsWebApiExplorerInstalled())
                return;

            ShowAndEnableCommand(menuCommand, true);
        }


        private void AddRamlContractCommandOnBeforeQueryStatus(object sender, EventArgs eventArgs)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            if (!IsAspNet5OrWebApiCoreInstalled())
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private void EditRamlPropertiesCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            ShowOrHideCommandRaml(sender);
        }

        private bool IsNetCoreApiExplorerInstalled()
        {
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.NetCoreApiExplorer");
            return isWebApiCoreInstalled;
        }

        private bool IsWebApiExplorerInstalled()
        {
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.WebApiExplorer");
            return isWebApiCoreInstalled;
        }

        private static bool IsAspNet5OrWebApiCoreInstalled()
        {
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                return IsAspNet5MvcInstalled(proj);

            return IsWebApiCoreInstalled(proj);
        }

        private static bool IsAJsonOrXProj()
        {
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            return VisualStudioAutomationHelper.IsJsonOrXProj(proj);
        }

        private static bool IsWebApiCoreInstalled(Project proj)
        {
            var componentModel = (IComponentModel) ServiceProvider.GlobalProvider.GetService(typeof (SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "Microsoft.AspNet.WebApi.Core");
            return isWebApiCoreInstalled;
        }

        private static bool IsAspNet5MvcInstalled(Project proj)
        {
            var componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            //net core app 2: "Microsoft.AspNetCore"
            return installerServices.IsPackageInstalled(proj, "Microsoft.AspNetCore.Mvc");
        }


        private void UpdateRamlRefCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            ShowOrHideUpdateRamlRefCommand(sender, RamlReferenceServiceBase.ApiReferencesFolderName);
        }

        private void ChangeCommandStatus(CommandID commandId, bool enable)
        {
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (mcs == null) return;

            var menuCmd = mcs.FindCommand(commandId);
            if (menuCmd != null) menuCmd.Enabled = enable;
        }

        private static void ShowOrHideCommandAddRefApiFolder(object sender)
        {
            var folderName = Settings.Default.ApiReferencesFolderName;

            ShowOrHideForFolder(sender, folderName);
        }

        private static void ShowOrHideForFolder(object sender, string folderName)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            if (IsInFolder(folderName)) return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private static bool IsInFolder(string folderName)
        {
            IVsHierarchy hierarchy;
            uint itemid;

            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return true;
            // Get the file path
            string itemFullPath;
            ((IVsProject) hierarchy).GetMkDocument(itemid, out itemFullPath);

            var folder = Path.GetDirectoryName(itemFullPath);

            if (!folder.EndsWith(folderName))
                return true;
            return false;
        }

        private static void ShowOrHideCommandAddContractFolder(object sender)
        {
            ShowOrHideCommandForFolder(sender, Settings.Default.ContractsFolderName);
        }

        private static void ShowOrHideCommandForFolder(object sender, string folderName)
        {
            // get the menu that fired the event
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            if (IsInFolder(folderName)) return;

            var dte = ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && !IsAspNet5MvcInstalled(proj))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private static void ShowOrHideUpdateRamlRefCommand(object sender, string containingFolderName)
        {
            // get the menu that fired the event
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            IVsHierarchy hierarchy;
            uint itemid;

            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            // Get the file path
            string itemFullPath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out itemFullPath);

            FileInfo transformFileInfo;
            try
            {
                transformFileInfo = new FileInfo(itemFullPath);
            }
            catch (Exception)
            {
                return;
            }

            var endsWithExtension = transformFileInfo.Name.EndsWith(".raml");

            // if not leave the menu hidden
            if (!endsWithExtension) return;

            if (!itemFullPath.Contains(containingFolderName))
                return;

            var folder = Path.GetDirectoryName(itemFullPath);
            if (folder.EndsWith(InstallerServices.IncludesFolderName))
                return;

            var refFile = InstallerServices.GetRefFilePath(itemFullPath);
            var source = RamlReferenceReader.GetRamlSource(refFile);
            if (string.IsNullOrWhiteSpace(source))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private static void ShowOrHideUpdateRamlCommandContract(object sender)
        {
            // get the menu that fired the event
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            IVsHierarchy hierarchy;
            uint itemid;

            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            // Get the file path
            string itemFullPath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out itemFullPath);

            FileInfo transformFileInfo;
            try
            {
                transformFileInfo = new FileInfo(itemFullPath);
            }
            catch (Exception)
            {
                return;
            }


            var endsWithExtension = transformFileInfo.Name.EndsWith(".raml");

            // if not leave the menu hidden
            if (!endsWithExtension) return;

            if (itemFullPath.Contains(RamlReferenceServiceBase.ApiReferencesFolderName))
                return;

            var folder = Path.GetDirectoryName(itemFullPath);
            if (folder.EndsWith(InstallerServices.IncludesFolderName))
                return;

            if (!IsAspNet5OrWebApiCoreInstalled())
                return;

            var refFile = InstallerServices.GetRefFilePath(itemFullPath);
            var source = RamlReferenceReader.GetRamlSource(refFile);
            if(string.IsNullOrWhiteSpace(source))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        private static void ShowOrHideCommandRaml(object sender)
        {
            // get the menu that fired the event
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            ShowAndEnableCommand(menuCommand, false);

            IVsHierarchy hierarchy;
            uint itemid;

            if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
            // Get the file path
            string itemFullPath;
            ((IVsProject)hierarchy).GetMkDocument(itemid, out itemFullPath);

            FileInfo transformFileInfo;
            try
            {
                transformFileInfo = new FileInfo(itemFullPath);
            }
            catch (Exception)
            {
                return;
            }

            var endsWithExtension = transformFileInfo.Name.EndsWith(".raml");

            // if not leave the menu hidden
            if (!endsWithExtension) return;

            var refFile = InstallerServices.GetRefFilePath(itemFullPath);
            if (!File.Exists(refFile))
                return;

            ShowAndEnableCommand(menuCommand, true);
        }

        //private static void ShowOrHideCommandReverseEngineering(object sender)
        //{
        //    // get the menu that fired the event
        //    var menuCommand = sender as OleMenuCommand;
        //    if (menuCommand == null) return;

        //    ShowAndEnableCommand(menuCommand, false);

        //    IVsHierarchy hierarchy;
        //    uint itemid;

        //    if (!IsSingleProjectItemSelection(out hierarchy, out itemid)) return;
        //    // Get the file path
        //    string itemFullPath;
        //    ((IVsProject)hierarchy).GetMkDocument(itemid, out itemFullPath);
        //    var transformFileInfo = new FileInfo(itemFullPath);

        //    var endsWithExtension = transformFileInfo.Name.EndsWith(".raml");

        //    // if not leave the menu hidden
        //    if (!endsWithExtension) return;

        //    if (itemFullPath.Contains(RamlReferenceService.ApiReferencesFolderName))
        //        return;

        //    var folder = Path.GetDirectoryName(itemFullPath);
        //    if (folder.EndsWith(InstallerServices.IncludesFolderName))
        //        return;

        //    if (!IsAspNet5OrWebApiCoreInstalled())
        //        return;

        //    ShowAndEnableCommand(menuCommand, true);
        //}

        private static void ShowAndEnableCommand(OleMenuCommand menuCommand, bool visible)
        {
            menuCommand.Visible = visible;
            menuCommand.Enabled = visible;
        }

        public static bool IsSingleProjectItemSelection(out IVsHierarchy hierarchy, out uint itemid)
        {
            hierarchy = null;
            itemid = VSConstants.VSITEMID_NIL;

            var monitorSelection = GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
            var solution = GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            if (monitorSelection == null || solution == null)
            {
                return false;
            }

            var hierarchyPtr = IntPtr.Zero;
            var selectionContainerPtr = IntPtr.Zero;

            try
            {
                IVsMultiItemSelect multiItemSelect;
                var hr = monitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainerPtr);

                if (ErrorHandler.Failed(hr) || hierarchyPtr == IntPtr.Zero || itemid == VSConstants.VSITEMID_NIL)
                {
                    // there is no selection
                    return false;
                }

                // multiple items are selected
                if (multiItemSelect != null) return false;

                // there is a hierarchy root node selected, thus it is not a single item inside a project

                if (itemid == VSConstants.VSITEMID_ROOT) return false;

                hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
                if (hierarchy == null) return false;

                Guid guidProjectId;

                if (ErrorHandler.Failed(solution.GetGuidOfProject(hierarchy, out guidProjectId)))
                {
                    return false; // hierarchy is not a project inside the Solution if it does not have a ProjectID Guid
                }

                // if we got this far then there is a single project item selected
                return true;
            }
            finally
            {
                if (selectionContainerPtr != IntPtr.Zero)
                {
                    Marshal.Release(selectionContainerPtr);
                }

                if (hierarchyPtr != IntPtr.Zero)
                {
                    Marshal.Release(hierarchyPtr);
                }
            }
        }

        public void OnCanceled()
        {
            StopProgressBar();
        }

        private void StopProgressBar()
        {
            if (attachingDialog == null)
                return;

            int canceled;
            attachingDialog.EndWaitDialog(out canceled);
            attachingDialog = null;
        }

        private void StartProgressBar(string title, string message, string progressMessage)
        {
            var dialogFactory = GetService(typeof(SVsThreadedWaitDialogFactory)) as IVsThreadedWaitDialogFactory;
            IVsThreadedWaitDialog2 dialog = null;
            if (dialogFactory != null)
            {
                dialogFactory.CreateInstance(out dialog);
            }

            attachingDialog = (IVsThreadedWaitDialog3)dialog;

            attachingDialog.StartWaitDialogWithCallback(title,
                message, string.Empty, null,
                progressMessage, true, 0,
                true, 0, 0, this);
        }

    }
}
