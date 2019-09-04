using System;
using System.ComponentModel.Design;
using System.Dynamic;
using AMF.Common;
using AMF.Common.ViewModels;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using EnvDTE;

namespace AMF.Tools
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AddContractCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("5cf85f6a-50ee-4fe6-b316-838cbeafff00");

        private static DTE _dte;


        private static void BeforeQueryStatus(object sender, EventArgs e)
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
            var proj = VisualStudioAutomationHelper.GetActiveProject(_dte);

            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                return CommandsUtil.IsAspNet5MvcInstalled(proj);

            return CommandsUtil.IsWebApiCoreInstalled(proj);
        }


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AddContractCommand Instance
        {
            get;
            private set;
        }

        // Asynchronous initialization
        public static async System.Threading.Tasks.Task InitializeAsync(AsyncPackage package, EnvDTE.DTE dte)
        {
            var cmdId = new CommandID(CommandSet, CommandId);
            _dte = dte;

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }


        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(AsyncPackage package, EnvDTE.DTE dte)
        {
            var cmdId = new CommandID(CommandSet, CommandId);
            _dte = dte;

            var serviceProvider = (IServiceProvider)package;
            var commandService = serviceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private static void MenuItemCallback(object sender, EventArgs e)
        {
            var ramlScaffoldUpdater = RamlScaffoldServiceBase.GetRamlScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider, ramlScaffoldUpdater.AddContract, "Add RAML Contract", true, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 600;

            AmfToolsPackage.WindowManager.ShowDialog(ramlChooserViewModel, null, settings);
        }
    }
}
