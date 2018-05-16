using System;
using System.ComponentModel.Design;
using System.Globalization;
using AMF.Common;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MuleSoft.RAML.Tools;
using NuGet.VisualStudio;

namespace AMF.Tools
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ExtractRAMLCommand : IVsThreadedWaitDialogCallback
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("a72f02a8-cd0c-419f-b2d5-d0b11f14beb4");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractRAMLCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private ExtractRAMLCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, false);

            var dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (!VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!CommandsUtil.IsWebApiCoreInstalled(proj) || IsWebApiExplorerInstalled()))
                return;

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!CommandsUtil.IsAspNet5MvcInstalled(proj) || IsNetCoreApiExplorerInstalled()))
                return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, true);
        }

        private bool IsNetCoreApiExplorerInstalled()
        {
            var dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.NetCoreApiExplorer");
            return isWebApiCoreInstalled;
        }

        private bool IsWebApiExplorerInstalled()
        {
            var dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.WebApiExplorer");
            return isWebApiCoreInstalled;
        }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ExtractRAMLCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ExtractRAMLCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            StartProgressBar("Enable RAML metadata output", "Installing...", "Processing...");

            var service = ReverseEngineeringServiceBase.GetReverseEngineeringService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);
            service.AddReverseEngineering();

            StopProgressBar();

            System.Diagnostics.Process.Start("https://github.com/mulesoft-labs/raml-dotnet-tools#metadata-extract-a-raml-definition-from-your-web-app");
        }

        private IVsThreadedWaitDialog3 attachingDialog;

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
            var dialogFactory = this.ServiceProvider.GetService(typeof(SVsThreadedWaitDialogFactory)) as IVsThreadedWaitDialogFactory;
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

        public void OnCanceled()
        {
            throw new NotImplementedException();
        }
    }
}
