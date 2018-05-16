using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using AMF.Common;
using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Tools
{
    public class CodeGenerator
    {
        private static readonly char[] PossibleDirectorySeparatorChars = { Path.DirectorySeparatorChar, '\\', '/' };
        private readonly IT4Service t4Service;
        private readonly TemplatesManager templatesManager = new TemplatesManager();

        public CodeGenerator(IT4Service t4Service)
        {
            this.t4Service = t4Service;
        }

        public void GenerateCodeFromTemplate<T>(TemplateParams<T> templateParams) where T : IHasName
        {
            if (!Directory.Exists(templateParams.TargetFolder))
                Directory.CreateDirectory(templateParams.TargetFolder);

            foreach (var parameter in templateParams.ParameterCollection)
            {
                var generatedFileName = GetGeneratedFileName(templateParams.Suffix, templateParams.Prefix, parameter);
                var destinationFile = Path.Combine(templateParams.TargetFolder, generatedFileName);

                var result = t4Service.TransformText(templateParams.TemplatePath, templateParams.ParameterName, parameter, templateParams.BinPath, templateParams.TargetNamespace,
                    templateParams.UseAsyncMethods, templateParams.IncludeHasModels, templateParams.HasModels, templateParams.IncludeApiVersionInRoutePrefix, templateParams.ApiVersion);

                var contents = templatesManager.AddServerMetadataHeader(result.Content, Path.GetFileNameWithoutExtension(templateParams.TemplatePath), templateParams.Title);

                if (templateParams.Ovewrite || !File.Exists(destinationFile))
                {
                    File.WriteAllText(destinationFile, contents);
                }

                if (templateParams.ParameterName == "controllerObject" || templateParams.TargetFolder == templateParams.FolderPath)
                {
                    // add file if it does not exist
                    var fileItem = templateParams.ProjItem.ProjectItems.Cast<ProjectItem>()
                        .FirstOrDefault(i => i.Name == generatedFileName);
                    if (fileItem != null) continue;

                    // var alreadyIncludedInProj = IsAlreadyIncludedInProject(templateParams.FolderPath, templateParams.FolderItem, generatedFileName, templateParams.ProjItem);
                    //if (!alreadyIncludedInProj)
                    if(!VisualStudioAutomationHelper.IsJsonOrXProj(templateParams.FolderItem.ContainingProject))
                        templateParams.ProjItem.ProjectItems.AddFromFile(destinationFile);
                }
                else
                {
                    var folder = templateParams.TargetFolder.TrimEnd(Path.DirectorySeparatorChar);
                    var proj = templateParams.ProjItem.ContainingProject;
                    AddItem(folder, proj, generatedFileName, destinationFile, templateParams.RelativeFolder);
                }
            }
        }

        private static void AddItem(string folder, Project proj, string generatedFileName, string destinationFile, string relativeFolder)
        {
            var folderItem = CreateFolderItem(proj, folder, relativeFolder);

            if (folderItem == null)
                return;

            var fileItem = folderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == generatedFileName);
            if (fileItem != null) return;

            folderItem.ProjectItems.AddFromFile(destinationFile);
        }

        private static ProjectItem CreateFolderItem(Project proj, string path, string relativeFolder)
        {
            if (ContainsSubFolders(relativeFolder))
            {
                var folders = relativeFolder.Split(PossibleDirectorySeparatorChars);
                var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(proj, folders[0]);
                return CreateFolderItem(folderItem, folders, 1);
            }

            if (VisualStudioAutomationHelper.IsJsonOrXProj(proj))
                return null;

            var folderName = path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            return VisualStudioAutomationHelper.AddFolderIfNotExists(proj, folderName);
        }

        private static ProjectItem CreateFolderItem(ProjectItem projItem, IReadOnlyList<string> folders, int index)
        {
            var folderItem = VisualStudioAutomationHelper.AddFolderIfNotExists(projItem, folders[index]);
            index++;
            if (index >= folders.Count)
                return folderItem;

            return CreateFolderItem(folderItem, folders, index);
        }

        private static bool ContainsSubFolders(string folder)
        {
            if (folder == null)
                return false;
            return PossibleDirectorySeparatorChars.Any(folder.Contains);
        }

        private static string GetGeneratedFileName<T>(string suffix, string prefix, T parameter) where T : IHasName
        {
            var name = parameter.Name;
            if (!string.IsNullOrWhiteSpace(prefix))
                name = prefix + name;
            if (!string.IsNullOrWhiteSpace(suffix))
                name += suffix;

            var generatedFileName = name + ".cs";
            return generatedFileName;
        }


    }
}