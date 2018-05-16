using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using AMF.Tools.Properties;

namespace AMF.Tools
{
    public class TemplatesManager
    {
        private const string DefaultTemplateVersion = "0.1";
        private const int HashLineIndex = 2;
        private const int VersionLineIndex = 3;
        private const string HashPrefix = "// hash:";
        private const string TitlePrefix = "// title:";
        private const string VersionPrefix = "// version:";
        private readonly string ServerTemplatesVersion = Settings.Default.ServerTemplatesVersion;
        private readonly string ClientTemplatesVersion = Settings.Default.ClientTemplatesVersion;

        public void CopyServerTemplateToProjectFolder(string generatedFolderPath, string templateName, string title, string templateSubFolder)
        {
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;
            var sourceTemplateFolder = Path.Combine(extensionPath,
                "Templates" + Path.DirectorySeparatorChar + templateSubFolder + Path.DirectorySeparatorChar);

            CopyTemplateToProjectFolder(generatedFolderPath, templateName, sourceTemplateFolder, ServerTemplatesVersion, title);
        }

        public void CopyClientTemplateToProjectFolder(string generatedFolderPath)
        {
            var templateName = Settings.Default.ClientT4TemplateName;
            var extensionPath = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;
            CopyTemplateToProjectFolder(generatedFolderPath, templateName, extensionPath, ClientTemplatesVersion, Settings.Default.ClientTemplateTitle);
        }

        public bool ConfirmWhenIncompatibleClientTemplate(string generatodFolderPath)
        {
            return ConfirmWhenIncompatibleTemplate(generatodFolderPath, Settings.Default.ClientT4TemplateName, ClientTemplatesVersion);
        }

        public bool ConfirmWhenIncompatibleServerTemplate(string generatodFolderPath, string[] templates)
        {
            return templates.All(template => ConfirmWhenIncompatibleTemplate(generatodFolderPath, template, ServerTemplatesVersion));
        }

        private bool ConfirmWhenIncompatibleTemplate(string generatedFolderPath, string templateName, string version)
        {
            var templatesFolder = Path.Combine(generatedFolderPath, "Templates");
            var destTemplateFilePath = Path.Combine(templatesFolder, templateName);
            if (!File.Exists(destTemplateFilePath) || !HasTemplateChanged(destTemplateFilePath))
                return true;

            if (IsVersionCompatible(destTemplateFilePath, version))
                return true;

            var dialogResult = MessageBox.Show(
                string.Format(
                    "The current tool is not compatible with this version of the template {0}."
                    + " We need to overwrite it with the new version. Do you want to proceed?.", templateName),
                "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return dialogResult == MessageBoxResult.Yes;
        }

        private void CopyTemplateToProjectFolder(string generatedFolderPath, string templateName, string templatesSourceFolder, string version, string title)
        {
            var templatesFolder = Path.Combine(generatedFolderPath, "Templates");
            if (!Directory.Exists(templatesFolder))
                Directory.CreateDirectory(templatesFolder);

            var destTemplateFilePath = Path.Combine(templatesFolder, templateName);
            var sourceTemplateFilePath = Path.Combine(templatesSourceFolder, templateName);

            if (File.Exists(destTemplateFilePath))
            {
                ManageExistingTemplate(templateName, destTemplateFilePath, sourceTemplateFilePath, version, title);
            }
            else
            {
                CopyTemplateAndAddMetadata(sourceTemplateFilePath, destTemplateFilePath, version, title);
            }
        }

        private void ManageExistingTemplate(string templateName, string destTemplateFilePath, string sourceTemplateFilePath, string version, string title)
        {
            if (HasMetadata(destTemplateFilePath))
            {
                ManageExistingTemplateWithMetadata(templateName, destTemplateFilePath, sourceTemplateFilePath, version, title);
            }
            else
            {
                CopyTemplateAndAddMetadata(sourceTemplateFilePath, destTemplateFilePath, version, title);
            }
        }

        private void ManageExistingTemplateWithMetadata(string templateName, string destTemplateFilePath,
            string sourceTemplateFilePath, string version, string title)
        {
            if (HasTemplateChanged(destTemplateFilePath))
            {
                ManageTemplateThatWasCustomized(templateName, destTemplateFilePath, sourceTemplateFilePath, version, title);
            }
            else
            {
                CopyTemplateAndAddMetadata(sourceTemplateFilePath, destTemplateFilePath, version, title);
            }
        }

        private void ManageTemplateThatWasCustomized(string templateName, string destTemplateFilePath,
            string sourceTemplateFilePath, string version, string title)
        {
            if (IsTheSameVersion(destTemplateFilePath, version))
                return;

            if (IsVersionCompatible(destTemplateFilePath, version))
            {
                var text = string.Format("Template {0} has changed. Do you want to override it and loose your changes?",
                    templateName);
                var dialogResult = MessageBox.Show(text, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (dialogResult == MessageBoxResult.Yes)
                    CopyTemplateAndAddMetadata(sourceTemplateFilePath, destTemplateFilePath, version, title);
            }
            else
            {
                CopyTemplateAndAddMetadata(sourceTemplateFilePath, destTemplateFilePath, version, title);
            }
        }

        private bool IsVersionCompatible(string templateFilePath, string newTemplatesVersion)
        {
            var installedVersion = GetVersion(templateFilePath);
            var numbers = installedVersion.Split('.');
            var mayorInstalled = Convert.ToInt16(numbers[0]);
            var newMayor = Convert.ToInt16(newTemplatesVersion.Split('.')[0]);
            return mayorInstalled == newMayor;
        }

        private string GetVersion(string templateFilePath)
        {
            var lines = File.ReadAllLines(templateFilePath).ToList();
            if (!lines[VersionLineIndex].Contains(VersionPrefix))
                return DefaultTemplateVersion;

            return lines[VersionLineIndex].Replace(VersionPrefix + " ", string.Empty).Trim();
        }

        private void CopyTemplateAndAddMetadata(string sourceTemplateFilePath, string destTemplateFilePath, string version, string title)
        {
            File.Copy(sourceTemplateFilePath, destTemplateFilePath, true);
            AddTemplateMetadata(version, destTemplateFilePath, title);
        }

        private bool IsTheSameVersion(string templateFilePath, string version)
        {
            var currentVersion = GetVersion(templateFilePath);
            return currentVersion == version;
        }

        private bool HasTemplateChanged(string templateFilePath)
        {
            var lines = File.ReadAllLines(templateFilePath).ToList();
            if (!lines[HashLineIndex].Contains(HashPrefix))
                return false;

            var hash = lines[HashLineIndex].Replace(HashPrefix + " ", string.Empty);
            RemoveMetadataLines(lines);
            var contentWithoutMetadata = string.Join(Environment.NewLine, lines);
            bool hasChanged;

            using (var md5 = MD5.Create())
            {
                hasChanged = !VerifyMd5Hash(md5, contentWithoutMetadata, hash);
            }
            return hasChanged;
        }

        private static bool VerifyMd5Hash(HashAlgorithm md5, string input, string hash)
        {
            var hashOfInput = GetMd5Hash(md5, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        private static void RemoveMetadataLines(List<string> lines)
        {
            lines.RemoveAt(0);
            lines.RemoveAt(0);
            lines.RemoveAt(0);
            lines.RemoveAt(0);
            lines.RemoveAt(0);
        }

        private bool HasMetadata(string templateFilePath)
        {
            var lines = File.ReadAllLines(templateFilePath).ToList();
            return lines[HashLineIndex].Contains(HashPrefix);
        }

        private static string ComputeHash(string templateFilePath)
        {
            var lines = File.ReadAllLines(templateFilePath).ToList();
            if (lines[HashLineIndex].Contains(HashPrefix))
            {
                RemoveMetadataLines(lines);
            }
            var contentWithoutMetadata = string.Join(Environment.NewLine, lines);

            string hash;
            using (var md5 = MD5.Create())
            {
                hash = GetMd5Hash(md5, contentWithoutMetadata);
            }

            return hash;
        }

        private void AddTemplateMetadata(string version, string templateFilePath, string title)
        {
            var lines = File.ReadAllLines(templateFilePath).ToList();
            if (lines[HashLineIndex].Contains(HashPrefix))
            {
                RemoveMetadataLines(lines);
            }
            lines.Insert(0, "#>");
            lines.Insert(0, VersionPrefix + " " + version);
            lines.Insert(0, HashPrefix + " " + ComputeHash(templateFilePath));
            lines.Insert(0, TitlePrefix + " " + title);
            lines.Insert(0, "<#");
            
            File.WriteAllLines(templateFilePath, lines);
        }

        private static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public string AddServerMetadataHeader(string contents, string templateName, string title)
        {
            var header = string.Format("// Template: {0} ({1}.t4) version {2}{3}", title, templateName, ServerTemplatesVersion,
                Environment.NewLine);
            contents = contents.Insert(0, header);
            return contents;
        }

        public string AddClientMetadataHeader(string contents)
        {
            var header = string.Format("// Template: {0} ({1}) version {2}{3}", Settings.Default.ClientTemplateTitle,
                Settings.Default.ClientT4TemplateName, ClientTemplatesVersion, Environment.NewLine);
            contents = contents.Insert(0, header);
            return contents;
        }

    }
}