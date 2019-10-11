using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using EnvDTE;

namespace Raml.Common
{
    public class InstallerServices
    {
        public const string IncludesFolderName = "includes";

        public static string AddRefFile(string ramlSourceFile, string destFolderPath, string targetFileName, RamlProperties properties)
        {
            var refFileName = Path.GetFileNameWithoutExtension(targetFileName) + ".ref";
            var refFilePath = Path.Combine(destFolderPath, refFileName);
            var content = RamlPropertiesManager.BuildContent(properties);

            if (File.Exists(refFilePath))
                new FileInfo(refFilePath).IsReadOnly = false;

            File.WriteAllText(refFilePath, content);
            new FileInfo(refFilePath).IsReadOnly = true;

            return refFilePath;
        }


        public static string GetRefFilePath(string ramlFilePath)
        {
            var containingFolder = Path.GetDirectoryName(ramlFilePath);
            if (containingFolder == null) throw new InvalidOperationException("Could not get folder of RAML file " + ramlFilePath);

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(ramlFilePath);
            var refFileName = fileNameWithoutExtension + ".ref";

            var refFilePath = Path.Combine(containingFolder, refFileName);

            if (File.Exists(refFilePath))
                return refFilePath;

            refFilePath = Path.Combine(Path.Combine(containingFolder, fileNameWithoutExtension), refFileName);

            return refFilePath;
        }

        public static void RemoveSubItemsAndAssociatedFiles(ProjectItem parentItem)
        {
            if (parentItem == null) return;

            var projectItems = parentItem.ProjectItems.Cast<ProjectItem>();
            RemoveSubItemsAndAssociatedFiles(projectItems);
        }

        public static void RemoveSubItemsAndAssociatedFiles(IEnumerable<ProjectItem> projectItems)
        {
            foreach (var item in projectItems)
            {
                item.Remove();
                var filePath = item.FileNames[0];
                new FileInfo(filePath).IsReadOnly = false;
                File.Delete(filePath);
            }
        }

        public static void AddNewIncludedFiles(RamlIncludesManagerResult result, ProjectItem includesFolderItem,
            ProjectItem destFolderItem)
        {
            if (!result.IncludedFiles.Any())
                return;

            if (includesFolderItem == null)
                includesFolderItem = destFolderItem.ProjectItems.AddFolder(InstallerServices.IncludesFolderName);

            foreach (var file in result.IncludedFiles)
            {
                includesFolderItem.ProjectItems.AddFromFile(file);
            }
        }


        public static ProjectItem AddOrUpdateRamlFile(string ramlSourceFile, string destFolderPath, ProjectItem destFolderItem, string targetFileName)
        {
            var ramlDestFile = Path.Combine(destFolderPath, targetFileName);

            CopyRamlFileToProjectFolder(ramlSourceFile, ramlDestFile);
            var ramlProjItem = destFolderItem.ProjectItems.AddFromFile(ramlDestFile);
            return ramlProjItem;
        }

        private static void CopyRamlFileToProjectFolder(string ramlSourceFile, string ramlDestFile)
        {
            if (File.Exists(ramlDestFile))
                new FileInfo(ramlDestFile).IsReadOnly = false;

            if (ramlSourceFile != ramlDestFile)
                File.Copy(ramlSourceFile, ramlDestFile, true);
        }

        public static MessageBoxResult ShowConfirmationDialog(string fileName)
        {
            var dialogResult = MessageBox.Show(fileName + " already exists. Do you want to override it ?", "Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            return dialogResult;
        }

    }
}