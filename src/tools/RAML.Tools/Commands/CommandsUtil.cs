using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio;

namespace AMF.Tools
{
    public class CommandsUtil
    {
        public static void ShowAndEnableCommand(OleMenuCommand menuCommand, bool visible)
        {
            menuCommand.Visible = visible;
            menuCommand.Enabled = visible;
        }

        public static bool IsWebApiCoreInstalled(Project proj)
        {
            var package = "Microsoft.AspNet.WebApi.Core";
            return IsPackageInstalled(proj, package);
        }

        public static bool IsPackageInstalled(Project proj, string package)
        {
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var isWebApiCoreInstalled = installerServices.IsPackageInstalled(proj, package);
            return isWebApiCoreInstalled;
        }

        public static bool IsAspNet5MvcInstalled(Project proj)
        {
            var componentModel = (IComponentModel)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            
            if (installerServices.IsPackageInstalled(proj, "Microsoft.AspNetCore.Mvc"))
                return true;

            //net core app 2
            if (installerServices.IsPackageInstalled(proj, "Microsoft.AspNetCore.All"))
                return true;

            // net core app 2.2
            if (installerServices.IsPackageInstalled(proj, "Microsoft.AspNetCore.App"))
                return true;

            return false;
        }

    }
}
