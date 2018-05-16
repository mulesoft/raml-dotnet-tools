using System.IO;
using EnvDTE;

namespace AMF.Tools
{
    public class TargetFolderResolver
    {
        public static string GetImplementationControllersFolderPath(Project proj, string implementationControllersFolder)
        {
            if (!string.IsNullOrWhiteSpace(implementationControllersFolder))
                return Path.Combine(Path.GetDirectoryName(proj.FullName), implementationControllersFolder);

            return Path.GetDirectoryName(proj.FullName) + Path.DirectorySeparatorChar + "Controllers" +
                   Path.DirectorySeparatorChar;
        }

        public static string GetBaseAndInterfacesControllersTargetFolder(Project proj, string targetFolderPath, string baseControllersFolder)
        {
            if(!string.IsNullOrWhiteSpace(baseControllersFolder))
                return Path.Combine(Path.GetDirectoryName(proj.FullName), baseControllersFolder);

            return targetFolderPath;
        }

        public static string GetModelsTargetFolder(Project project, string targetFolderPath, string modelsFolder)
        {
            if (!string.IsNullOrWhiteSpace(modelsFolder))
                return Path.Combine(Path.GetDirectoryName(project.FullName), modelsFolder);

            return targetFolderPath;
        }
    }
}
