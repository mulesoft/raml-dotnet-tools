using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using AMF.Tools.Properties;
using NuGet.VisualStudio;
using AMF.Common;

namespace AMF.Tools
{
    public class ReverseEngineeringAspNetCore3 : ReverseEngineeringAspNetCore
    {
        public ReverseEngineeringAspNetCore3(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void ConfigureNetCoreMvcServices(List<string> lines)
        {
            if (lines.Any(l => l.Contains("ConfigureServices(IServiceCollection services")))
            {
                var configuredServices = "            services.AddScoped(typeof(RAML.WebApiExplorer.ApiExplorerDataFilter));" + Environment.NewLine;
                if (!lines.Any(l => l.Contains("services.AddControllersWithViews")))
                {
                    configuredServices += "               services.AddControllersWithViews(options => {" + Environment.NewLine;
                    configuredServices += "                   options.Filters.AddService(typeof(RAML.WebApiExplorer.ApiExplorerDataFilter));" + Environment.NewLine;
                    configuredServices += "                   options.Conventions.Add(new RAML.WebApiExplorer.ApiExplorerVisibilityEnabledConvention());" + Environment.NewLine;
                    configuredServices += "                   options.Conventions.Add(new RAML.WebApiExplorer.ApiExplorerVisibilityDisabledConvention(typeof(RAML.WebApiExplorer.RamlController)));" + Environment.NewLine;
                    configuredServices += "               });" + Environment.NewLine;
                }
                var lineIndex = TextFileHelper.FindLineWith(lines, "ConfigureServices(IServiceCollection services");
                if (lineIndex > 0)
                    lines.Insert(lineIndex + 2, configuredServices);
            }

            if (!lines.Any(l => l.Contains("services.AddControllers();")))
                return;

            var line = TextFileHelper.FindLineWith(lines, "services.AddControllers();");
            lines[line] = lines[line].Replace("services.AddControllers();", "//services.AddControllers();");
        }

        protected override void InstallNetCoreDependencies(Project proj, IVsPackageMetadata[] packs, IVsPackageInstaller installer, IVsPackageInstallerServices installerServices)
        {
            // RAML.Parser.Expressions
            if (!installerServices.IsPackageInstalled(proj, RamlParserExpressionsPackageId))
            {
                installer.InstallPackage(NugetPackagesSource, proj, RamlParserExpressionsPackageId, RamlParserExpressionsPackageVersion, false);
            }

            // RAML.NetCoreAPIExplorer
            if (!installerServices.IsPackageInstalled(proj, RamlNetCoreApiExplorerPackageId))
            {
                installer.InstallPackage(NugetPackagesSource, proj, RamlNetCoreApiExplorerPackageId, RamlNetCoreApiExplorerPackageVersion, false);
            }
        }
    }
}