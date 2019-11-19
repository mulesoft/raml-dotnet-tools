using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AMF.Common;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using NuGet.VisualStudio;
using Task = System.Threading.Tasks.Task;

namespace AMF.Tools.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ExtractRAMLCommand : IVsThreadedWaitDialogCallback
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("a68674ed-a933-4709-a663-ae979526c056");
        private static DTE _dte;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtractRAMLCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ExtractRAMLCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            menuItem.BeforeQueryStatus += BeforeQueryStatus;
            commandService.AddCommand(menuItem);
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
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
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
        public static async Task InitializeAsync(AsyncPackage package, DTE dte)
        {
            _dte = dte;
            // Switch to the main thread - the call to AddCommand in ExtractRAMLCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new ExtractRAMLCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            try
            {
                package.JoinableTaskFactory.Run(() => StartProgressBarAsync("Enable RAML metadata output", "Installing...", "Processing..."));

                var service = ReverseEngineeringServiceBase.GetReverseEngineeringService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);
                service.AddReverseEngineering();
                System.Diagnostics.Process.Start("https://github.com/mulesoft-labs/raml-dotnet-tools#metadata-extract-a-raml-definition-from-your-web-app");
            }
            finally
            {
                StopProgressBar();
            }
        }

        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, false);

            var proj = VisualStudioAutomationHelper.GetActiveProject(_dte);

            if (!VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!CommandsUtil.IsWebApiCoreInstalled(proj) || IsWebApiExplorerInstalled()))
                return;

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj) && (!CommandsUtil.IsAspNet5MvcInstalled(proj) || IsNetCoreApiExplorerInstalled()))
                return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, true);
        }

        private bool IsNetCoreApiExplorerInstalled()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var proj = VisualStudioAutomationHelper.GetActiveProject(_dte);
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.NetCoreAPIExplorer");
            return isWebApiCoreInstalled;
        }

        private bool IsWebApiExplorerInstalled()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var proj = VisualStudioAutomationHelper.GetActiveProject(_dte);
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, "RAML.WebApiExplorer");
            return isWebApiCoreInstalled;
        }


        private IVsThreadedWaitDialog3 attachingDialog;

        private async Task StartProgressBarAsync(string title, string message, string progressMessage)
        {
            await package.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dialogFactory = await this.ServiceProvider.GetServiceAsync(typeof(SVsThreadedWaitDialogFactory)) as IVsThreadedWaitDialogFactory;
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

        private void StopProgressBar()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (attachingDialog == null)
                return;

            int canceled;
            attachingDialog.EndWaitDialog(out canceled);
            attachingDialog = null;
        }

        public void OnCanceled()
        {
        }
    }
}
