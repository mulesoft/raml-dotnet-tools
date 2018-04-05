using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using MuleSoft.RAML.Tools.Properties;
using NuGet.VisualStudio;
using Raml.Common;

namespace MuleSoft.RAML.Tools
{
    public class ReverseEngineeringServiceWebApi : ReverseEngineeringServiceBase
    {
        private static readonly string RamlWebApiExplorerPackageId = Settings.Default.RAMLWebApiExplorerPackageId;
        private static readonly string RamlWebApiExplorerPackageVersion = Settings.Default.RAMLWebApiExplorerPackageVersion;
        private static readonly string RamlParserPackageId = Settings.Default.RAMLParserPackageId;
        private static readonly string RamlParserPackageVersion = Settings.Default.RAMLParserPackageVersion;
        private static readonly string RamlApiCorePackageId = Settings.Default.RAMLApiCorePackageId;
        private static readonly string RamlApiCorePackageVersion = Settings.Default.RAMLApiCorePackageVersion;
        private static readonly string NewtonsoftJsonPackageId = Settings.Default.NewtonsoftJsonPackageId;
        private static readonly string NewtonsoftJsonPackageVersion = Settings.Default.NewtonsoftJsonPackageVersion;
        private static readonly string EdgePackageId = Settings.Default.EdgePackageId;
        private static readonly string EdgePackageVersion = Settings.Default.EdgePackageVersion;
        
        public ReverseEngineeringServiceWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override void ConfigureProject(Project proj)
        {
            AddXmlCommentsDocumentation(proj);
            ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "XML comments documentation added");
        }

        protected override void InstallDependencies(Project proj, IVsPackageMetadata[] packs, IVsPackageInstaller installer,
            IVsPackageInstallerServices installerServices)
        {
            InstallWebApiDependencies(proj, packs, installer, installerServices);
        }

        protected override void RemoveConfiguration(Project proj)
        {
            RemovXmlCommentsDocumentation(proj);
            ActivityLog.LogInformation(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, "XML comments documentation removed");
        }

        protected override void RemoveDependencies(Project proj, IVsPackageInstallerServices installerServices, IVsPackageUninstaller installer)
        {
            // Uninstall RAML.WebApiExplorer
            if (installerServices.IsPackageInstalled(proj, RamlWebApiExplorerPackageId))
            {
                installer.UninstallPackage(proj, RamlWebApiExplorerPackageId, false);
            }
        }

        private void AddXmlCommentsDocumentation(Project proj)
        {
            ConfigureXmlDocumentationFileInProject(proj);
            AddIncludeXmlCommentsInWebApiConfig(proj);
        }

        private static void ConfigureXmlDocumentationFileInProject(Project proj)
        {
            var config = proj.ConfigurationManager.ActiveConfiguration;
            var configProps = config.Properties;
            var prop = configProps.Item("DocumentationFile");
            prop.Value = string.Format("bin\\{0}.XML", proj.Name);
        }

        private static void AddIncludeXmlCommentsInWebApiConfig(Project proj)
        {
            var appStart = proj.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "App_Start");
            if (appStart == null) return;

            var webApiConfig = appStart.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "WebApiConfig.cs");
            if (webApiConfig == null) return;

            var path = webApiConfig.FileNames[0];
            var lines = File.ReadAllLines(path).ToList();

            if (lines.Any(l => l.Contains("DocumentationProviderConfig.IncludeXmlComments")))
                return;

            InsertLine(lines);

            File.WriteAllText(path, string.Join(Environment.NewLine, lines));
        }


        private static void InsertLine(List<string> lines)
        {
            var line = TextFileHelper.FindLineWith(lines, "Register(HttpConfiguration config)");
            var inserted = false;
            if (line != -1)
            {
                if (lines[line + 1].Contains("{"))
                {
                    lines.Insert(line + 2, "\t\t\tRAML.WebApiExplorer.DocumentationProviderConfig.IncludeXmlComments();");
                    inserted = true;
                }
            }

            if (!inserted)
            {
                line = TextFileHelper.FindLineWith(lines, ".MapHttpAttributeRoutes();");
                if (line != -1)
                    lines.Insert(line + 1, "\t\t\tRAML.WebApiExplorer.DocumentationProviderConfig.IncludeXmlComments();");
            }
        }

        private void InstallWebApiDependencies(Project proj, IVsPackageMetadata[] packs, IVsPackageInstaller installer,
            IVsPackageInstallerServices installerServices)
        {
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, NewtonsoftJsonPackageId,
                NewtonsoftJsonPackageVersion, Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, EdgePackageId, EdgePackageVersion,
                Settings.Default.NugetExternalPackagesSource);
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, "System.ComponentModel.Annotations",
                "4.0.0", Settings.Default.NugetExternalPackagesSource);

            // RAML.Parser
            if (!installerServices.IsPackageInstalled(proj, RamlParserPackageId))
            {
                installer.InstallPackage(NugetPackagesSource, proj, RamlParserPackageId, RamlParserPackageVersion,
                    false);
            }

            // RAML.Api.Core
            if (!installerServices.IsPackageInstalled(proj, RamlApiCorePackageId))
            {
                //installer.InstallPackage(nugetPackagesSource, proj, ramlApiCorePackageId, ramlApiCorePackageVersion, false);
                installer.InstallPackage(NugetPackagesSource, proj, RamlApiCorePackageId, RamlApiCorePackageVersion,
                    false);
            }

            // RAML.WebApiExplorer
            if (!installerServices.IsPackageInstalled(proj, RamlWebApiExplorerPackageId))
            {
                installer.InstallPackage(NugetPackagesSource, proj, RamlWebApiExplorerPackageId,
                    RamlWebApiExplorerPackageVersion, false);
            }
        }

        private void RemovXmlCommentsDocumentation(Project proj)
        {
            RemoveXmlCommentsInWebApiConfig(proj);
            RemoveXmlDocumentationFileInProject(proj);
        }

        private static void RemoveXmlCommentsInWebApiConfig(Project proj)
        {
            var appStart = proj.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "App_Start");
            if (appStart == null) return;

            var webApiConfig = appStart.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "WebApiConfig.cs");
            if (webApiConfig == null) return;

            var path = webApiConfig.FileNames[0];
            var content = File.ReadAllText(path);

            if (!content.Contains("DocumentationProviderConfig.IncludeXmlComments"))
                return;

            content = content.Replace("RAML.WebApiExplorer.DocumentationProviderConfig.IncludeXmlComments();", string.Empty);

            File.WriteAllText(path, content);
        }

        private static void RemoveXmlDocumentationFileInProject(Project proj)
        {
            var config = proj.ConfigurationManager.ActiveConfiguration;
            var configProps = config.Properties;
            var prop = configProps.Item("DocumentationFile");
            prop.Value = string.Empty;
        }
    }
}