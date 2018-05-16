using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using AMF.Common;

namespace AMF.Tools
{
    public class JsonSchemaMessagesManager
    {
        public static void AddJsonParsingErrors(IDictionary<string, string> warnings, ProjectItem contractsFolderItem, string targetFolderPath)
        {
            var fileName = "json-parsing-errors.txt";
            var file = Path.Combine(targetFolderPath, fileName);

            AddFileContents(warnings, file);

            AddFileToProject(contractsFolderItem, fileName, file);
        }

        private static void AddFileToProject(ProjectItem contractsFolderItem, string fileName, string file)
        {
            if (VisualStudioAutomationHelper.IsJsonOrXProj(contractsFolderItem.ContainingProject))
                return;

            var fileItem = contractsFolderItem.ProjectItems.Cast<ProjectItem>().FirstOrDefault(i => i.Name == fileName);
            if (fileItem != null) return;

            contractsFolderItem.ProjectItems.AddFromFile(file);
        }

        private static void AddFileContents(IDictionary<string, string> warnings, string file)
        {
            using (var sw = File.AppendText(file))
            {
                sw.WriteLine("-------" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "-------");
                foreach (var warning in warnings)
                {
                    sw.WriteLine(warning.Key + ": " + warning.Value + Environment.NewLine);
                }
            }
        }
    }
}