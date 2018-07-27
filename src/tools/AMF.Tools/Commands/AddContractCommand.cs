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

        /// <summary>
        /// VS AsyncPackage that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddContractCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private AddContractCommand(AsyncPackage package)
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
        public static AddContractCommand Instance
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
        public static void Initialize(AsyncPackage package)
        {
            Instance = new AddContractCommand(package);
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
            var ramlScaffoldUpdater = RamlScaffoldServiceBase.GetRamlScaffoldService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider);
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider, ramlScaffoldUpdater.AddContract, "Add RAML Contract", true, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 570;

            AmfToolsPackage.WindowManager.ShowDialog(ramlChooserViewModel, null, settings);
        }
    }
}
