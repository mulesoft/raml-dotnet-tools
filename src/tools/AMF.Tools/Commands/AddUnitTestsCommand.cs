using System;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AMF.Common;
using AMF.Common.ViewModels;
using AMF.Tools.Properties;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace AMF.Tools.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AddUnitTestsCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("97f1c567-cbc8-416c-a00d-07c10909d7d1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUnitTestsCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private AddUnitTestsCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
            menuItem.BeforeQueryStatus += BeforeQueryStatus;
            commandService.AddCommand(menuItem);
        }

        private void BeforeQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, false);

            if (!IsAspNet5OrWebApiCoreInstalled())
                return;

            CommandsUtil.ShowAndEnableCommand(menuCommand, true);
        }

        private static bool IsAspNet5OrWebApiCoreInstalled()
        {
            var dte = Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                return CommandsUtil.IsAspNet5MvcInstalled(proj);

            return CommandsUtil.IsWebApiCoreInstalled(proj);
        }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AddUnitTestsCommand Instance
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
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in AddUnitTestsCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new AddUnitTestsCommand(package, commandService);
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

            var unitTestsScaffoldService = UnitTestsScaffoldServiceBase.GetScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider, unitTestsScaffoldService.AddTests, "Add Unit Tests", 
                true, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 570;

            AmfToolsPackage.WindowManager.ShowDialog(ramlChooserViewModel, null, settings);
        }
    }
}
