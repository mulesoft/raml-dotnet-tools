using System;
using System.ComponentModel.Design;
using System.Dynamic;
using System.Globalization;
using AMF.Common.ViewModels;
using AMF.Tools.Properties;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MuleSoft.RAML.Tools;

namespace AMF.Tools
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class AddReferenceCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("0b89f7f1-319a-48f4-8ba7-90c8fbc0c062");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddReferenceCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private AddReferenceCommand(Package package)
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
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static AddReferenceCommand Instance
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
            Instance = new AddReferenceCommand(package);
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
            var generationServices = RamlReferenceServiceBase.GetRamlReferenceService(Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider, new ActivityLogger());
            var ramlChooserViewModel = new RamlChooserViewModel();
            ramlChooserViewModel.Load(this.ServiceProvider, generationServices.AddRamlReference, "Add RAML Reference", false, Settings.Default.RAMLExchangeUrl);
            dynamic settings = new ExpandoObject();
            settings.Height = 475;
            AmfToolsPackage.WindowManager.ShowDialog(ramlChooserViewModel, null, settings);
        }
    }
}
