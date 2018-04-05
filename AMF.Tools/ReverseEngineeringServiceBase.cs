using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using NuGet.VisualStudio;
using AMF.Common;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MuleSoft.RAML.Tools
{
    public abstract class ReverseEngineeringServiceBase
    {
        protected static readonly string NugetPackagesSource = Settings.Default.NugetPackagesSource;

        private readonly IServiceProvider serviceProvider;


        protected ReverseEngineeringServiceBase(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public static ReverseEngineeringServiceBase GetReverseEngineeringService(ServiceProvider serviceProvider)
        {
            var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);
            ReverseEngineeringServiceBase service;
            if (VisualStudioAutomationHelper.IsANetCoreProject(proj))
                service = new ReverseEngineeringAspNetCore(serviceProvider);
            else
                service = new ReverseEngineeringServiceWebApi(serviceProvider);
            return service;
        }

        public void AddReverseEngineering()
        {
            try
            {
                ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "Enable RAML metadata output process started");
                var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
                var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

                InstallNugetAndDependencies(proj);
                ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "Nuget packages and dependencies installed");

                ConfigureProject(proj);
                ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "Configuration finished");
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(ex));
                MessageBox.Show("Error when trying to enable RAML metadata output. " + ex.Message);
                throw;
            }
        }

        protected abstract void ConfigureProject(Project proj);

        private void InstallNugetAndDependencies(Project proj)
        {
            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();

            InstallDependencies(proj, packs, installer, installerServices);
        }

        protected abstract void InstallDependencies(Project proj, IVsPackageMetadata[] packs, IVsPackageInstaller installer,
            IVsPackageInstallerServices installerServices);


        public void ExtractRAML()
        {
            throw new NotImplementedException();
        }

        public void RemoveReverseEngineering()
        {
            try
            {
                ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "Disable RAML metadata output process started");
                var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
                var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

                UninstallNugetAndDependencies(proj);
                ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "Nuget package uninstalled");

                RemoveConfiguration(proj);
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(ex));
                MessageBox.Show("Error when trying to disable RAML metadata output. " + ex.Message);
                throw;
            }
        }

        protected abstract void RemoveConfiguration(Project proj);

        private void UninstallNugetAndDependencies(Project proj)
        {
            var componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageUninstaller>();

            RemoveDependencies(proj, installerServices, installer);
        }

        protected abstract void RemoveDependencies(Project proj, IVsPackageInstallerServices installerServices,
            IVsPackageUninstaller installer);
    }
}