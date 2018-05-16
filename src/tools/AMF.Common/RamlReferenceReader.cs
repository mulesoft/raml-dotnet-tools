using System;
using System.IO;

namespace AMF.Common
{
    public static class RamlReferenceReader
    {
        public static string GetRamlNamespace(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            return lines[2].Replace("namespace:", string.Empty).Trim();
        }

        public static string GetRamlSource(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var source = lines[1].Replace("source:", string.Empty).Trim();
            return source;
        }

        public static bool GetRamlUseAsyncMethods(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
            if (lines.Length <= 3)
                return false;

            var useAsync = lines[3].Replace("async:", string.Empty).Trim();
            
            if(string.IsNullOrWhiteSpace(useAsync))
                return false;

            bool result;
            bool.TryParse(useAsync, out result);
            return result;
        }

        public static bool GetRamlIncludeApiVersionInRoutePrefix(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 4)
                return false;

            var useAsync = lines[4].Replace("includeApiVersionInRoutePrefix:", string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(useAsync))
                return false;

            bool result;
            Boolean.TryParse(useAsync, out result);
            return result;
        }

        public static string GetClientRootClassName(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 3)
                return "ApiClient";

            var clientRootClassName = lines[3].Replace("client:", string.Empty).Trim();
            return clientRootClassName;
        }

        public static string GetModelsFolder(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 5)
                return null;

            var clientRootClassName = lines[5].Replace("modelsFolder:", string.Empty).Trim();
            return clientRootClassName;
        }

        public static string GetImplementationControllersFolder(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 6)
                return null;

            var clientRootClassName = lines[6].Replace("implementationControllersFolder:", string.Empty).Trim();
            return clientRootClassName;
        }

        public static bool GetAddGeneratedSuffix(string referenceFilePath)
        {
            var contents = File.ReadAllText(referenceFilePath);
            var lines = contents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 7)
                return false;

            var addGeneratedSuffix = lines[7].Replace("addGeneratedSuffix:", string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(addGeneratedSuffix))
                return false;

            bool result;
            bool.TryParse(addGeneratedSuffix, out result);
            return result;

        }
    }
}