using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using AMF.Tools.Properties;
using AMF.Common;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;

namespace AMF.Tools
{
    public class RamlScaffoldServiceWebApi : RamlScaffoldServiceBase
    {
        private readonly string newtonsoftJsonPackageVersion = Settings.Default.NewtonsoftJsonPackageVersion;
        private readonly string microsoftNetHttpPackageId = Settings.Default.MicrosoftNetHttpPackageId;
        private readonly string microsoftNetHttpPackageVersion = Settings.Default.MicrosoftNetHttpPackageVersion;
        private readonly string ramlApiCorePackageId = Settings.Default.RAMLApiCorePackageId;
        private readonly string ramlApiCorePackageVersion = Settings.Default.RAMLApiCorePackageVersion;

        public override string TemplateSubFolder
        {
            get { return "RAMLWebApi2Scaffolder"; }
        }

        public RamlScaffoldServiceWebApi(IT4Service t4Service, IServiceProvider serviceProvider): base(t4Service, serviceProvider){}

        protected void InstallDependencies(Project proj, string packageVersion)
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            var installerServices = componentModel.GetService<IVsPackageInstallerServices>();
            var installer = componentModel.GetService<IVsPackageInstaller>();

            var packs = installerServices.GetInstalledPackages(proj).ToArray();

            // AMF.Api.Core dependencies
            NugetInstallerHelper.InstallPackageIfNeeded(proj, packs, installer, microsoftNetHttpPackageId, microsoftNetHttpPackageVersion, Settings.Default.NugetExternalPackagesSource);

            InstallNugetDependencies(proj, packageVersion);

            // AMF.Api.Core
            if (!installerServices.IsPackageInstalled(proj, ramlApiCorePackageId))
            {
                installer.InstallPackage(nugetPackagesSource, proj, ramlApiCorePackageId, ramlApiCorePackageVersion, false);
            }
        }

        public override void AddContract(RamlChooserActionParams parameters)
        {
            Tracking.Track("Asp.Net WebApi Scaffold");

            var dte = ServiceProvider.GetService(typeof(SDTE)) as DTE;
            var proj = VisualStudioAutomationHelper.GetActiveProject(dte);

            InstallDependencies(proj, newtonsoftJsonPackageVersion);

            AddXmlFormatterInWebApiConfig(proj);

            var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, ContractsFolderName);
            var contractsFolderPath = Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + ContractsFolderName + Path.DirectorySeparatorChar;

            var targetFolderPath = GetTargetFolderPath(contractsFolderPath, parameters.TargetFileName);
            if (!Directory.Exists(targetFolderPath))
                Directory.CreateDirectory(targetFolderPath);

            if (string.IsNullOrWhiteSpace(parameters.RamlSource) && !string.IsNullOrWhiteSpace(parameters.RamlTitle))
            {
                AddEmptyContract(folderItem, contractsFolderPath, parameters);
            }
            else
            {
                AddContractFromFile(folderItem, contractsFolderPath, parameters);
            }
        }

        protected override string GetTargetFolderPath(string folderPath, string targetFilename)
        {
            return folderPath;
        }

        private static void AddXmlFormatterInWebApiConfig(Project proj)
        {
            var appStart = proj.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "App_Start");
            if (appStart == null) return;

            var webApiConfig = appStart.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == "WebApiConfig.cs");
            if (webApiConfig == null) return;

            var path = webApiConfig.FileNames[0];
            var lines = File.ReadAllLines(path).ToList();

            if (lines.Any(l => l.Contains("XmlSerializerFormatter")))
                return;

            InsertLine(lines);

            File.WriteAllText(path, string.Join(Environment.NewLine, lines));
        }

        private static void InsertLine(List<string> lines)
        {
            var line = FindLineWith(lines, "Register(HttpConfiguration config)");
            var inserted = false;

            if (line != -1)
            {
                if (lines[line + 1].Contains("{"))
                {
                    InsertLines(lines, line + 2);
                    inserted = true;
                }
            }

            if (inserted) return;

            line = FindLineWith(lines, ".MapHttpAttributeRoutes();");
            if (line != -1)
            {
                InsertLines(lines, line + 1);
            }
        }

        private static void InsertLines(IList<string> lines, int index)
        {
            lines.Insert(index, "\t\t\tconfig.Formatters.Remove(config.Formatters.XmlFormatter);");
            lines.Insert(index, "\t\t\tconfig.Formatters.Add(new AMF.Api.Core.XmlSerializerFormatter());");
        }

        private static int FindLineWith(IReadOnlyList<string> lines, string find)
        {
            var line = -1;
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains(find))
                    line = i;
            }
            return line;
        }
    }
}