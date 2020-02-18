using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace AMF.Common
{
    public class VisualStudioAutomationHelper
    {
        public const string RamlVsToolsActivityLogSource = "RamlVsTools";

        public static Project GetActiveProject(_DTE dte)
        {
            Project activeProject = null;

            var activeSolutionProjects = dte.ActiveSolutionProjects as Array;
            if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
            {
                activeProject = activeSolutionProjects.GetValue(0) as Project;
            }

            return activeProject;
        }

        public static IEnumerable<string> GetProjects(_DTE dte)
        {
            var projects = new List<string>();

            var solutionProjects = dte.Solution.Projects;
            if (solutionProjects == null)
                return projects;

            var projs = solutionProjects.Cast<Project>();
            if (projs == null)
                return projects;

            return projs.Select(p => p.Name).ToArray();
        }

        public static Project GetProject(_DTE dte, string projName)
        {
            var solutionProjects = dte.Solution.Projects;
            var projects = solutionProjects?.Cast<Project>();
            return projects?.FirstOrDefault(proj => proj?.Name == projName);
        }

        public static string GetDefaultNamespace(IServiceProvider serviceProvider)
        {
            var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
            var project = GetActiveProject(dte);

            return GetDefaultNamespace(project);
        }

        public static string GetDefaultNamespace(Project project)
        {
            var namespaceProperty = IsJsonOrXProj(project) ? "RootNamespace" : "DefaultNamespace";
            return project.Properties.Item(namespaceProperty).Value.ToString();
        }

        public static string GetExceptionInfo(Exception ex)
        {
            return ex.Message + Environment.NewLine + ex.StackTrace +
                   (ex.InnerException != null
                       ? Environment.NewLine + ex.InnerException.Message + Environment.NewLine +
                         ex.InnerException.StackTrace
                       : string.Empty);
        }

        public static ProjectItem AddFolderIfNotExists(Project proj, string folderName)
        {
            var path = Path.GetDirectoryName(proj.FullName) + "\\" + folderName + "\\";
            var projectItem = proj.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == folderName);
            if (projectItem != null)
                return projectItem;

            if (!IsJsonOrXProj(proj) && Directory.Exists(path))
                projectItem = proj.ProjectItems.AddFromDirectory(path);
            else if(!Directory.Exists(path))
                projectItem = proj.ProjectItems.AddFolder(folderName);

            return projectItem;
        }

        public static ProjectItem AddFolderIfNotExists(ProjectItem projItem, string folderName)
        {
            var path = Path.GetDirectoryName(projItem.FileNames[0]) + "\\" + folderName + "\\";
            var projectItem = projItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == folderName);
            if (projectItem != null)
                return projectItem;

            if (!IsJsonOrXProj(projItem.ContainingProject) && Directory.Exists(path))
                projectItem = projItem.ProjectItems.AddFromDirectory(path);
            else if (!Directory.Exists(path))
                projectItem = projItem.ProjectItems.AddFolder(folderName);

            return projectItem;
        }


        public static ProjectItem AddFolderIfNotExists(ProjectItem projItem, string folderName, string folderPath)
        {
            
            var projectItem = projItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == folderName);
            if (projectItem != null)
                return projectItem;

            if (Directory.Exists(folderPath))
                projectItem = projItem.ProjectItems.AddFromDirectory(folderPath);
            else
                projectItem = projItem.ProjectItems.AddFolder(folderName);

            return projectItem;
        }

        public static bool IsAnAspNetCore3Project(Project proj)
        {
            if (!IsANetCoreProject(proj))
                return false;

            var targetFramework = GetTargetFrameworkMoniker(proj);
            if (IsAspNet3x(targetFramework))
                return true;

            return false;
        }

        public static bool IsANetCore3Project(Project proj)
        {
            if (!IsANetCoreProject(proj))
                return false;

            var targetFramework = GetTargetFrameworkMoniker(proj);
            if (IsNetCore3(targetFramework))
                return true;

            return false;
        }

        private static bool IsNetCore3(string targetFramework)
        {
            return IsAspNet3x(targetFramework)
                || targetFramework.Contains("netstandard") && targetFramework.Contains("2.1");
        }

        private static bool IsAspNet3x(string targetFramework)
        {
            return targetFramework.Contains("netcore") && targetFramework.Contains("v3.");
        }

        public static bool IsANetCoreProject(Project proj)
        {
            if (proj.FileName.EndsWith("xproj") || proj.FileName.EndsWith("json"))
                return true;

            var targetFramework = GetTargetFrameworkMoniker(proj);
            if (targetFramework.Contains("netcore") || targetFramework.Contains("netstandard"))
                return true;

            if (targetFramework.Contains("netframework"))
                return false;

            throw new InvalidOperationException("Cannot determine .net framework. " + targetFramework);
        }

        public static string GetTargetFrameworkVersion(Project project)
        {
            var moniker = GetTargetFrameworkMoniker(project);
            var regex = new Regex(".+version=v([0-9]{1,2}(.[0-9]){0,2}(.[0-9]){0,2})", RegexOptions.IgnoreCase);
            var match = regex.Match(moniker);
            if (match.Success && match.Groups.Count > 1)
                return match.Groups[1].Value;

            return string.Empty;
        }
        public static string GetTargetFrameworkMoniker(Project proj)
        {
            //.NETCoreApp,Version = v2.0
            //.NETCoreApp,Version = v1.1
            //.NETFramework,Version = v4.6.1
            Property targetFwkObj;
            try
            {
                targetFwkObj = proj.Properties.Item("TargetFrameworkMoniker"); //var targetFwkObj = proj.Properties.Item("TargetFramework");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Cannot determine .net framework. TargetFrameworkMoniker not present.", ex);
            }
            var targetFramework = (targetFwkObj.Value as string).ToLowerInvariant();
            return targetFramework;
        }

        public static bool IsJsonOrXProj(Project proj)
        {
            if (proj.FileName.EndsWith("xproj") || proj.FileName.EndsWith("json"))
                return true;

            return false;
        }


    }
}