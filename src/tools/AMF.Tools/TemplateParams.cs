using System.Collections.Generic;
using EnvDTE;
using AMF.Tools.Core.WebApiGenerator;

namespace AMF.Tools
{
    public class TemplateParams<TT> where TT : IHasName
    {
        private string _testsNamespace;

        public TemplateParams(string templatePath, ProjectItem projItem, string parameterName,
            IEnumerable<TT> parameterCollection, string folderPath, ProjectItem folderItem, string binPath,
            string targetNamespace, string suffix = null, bool ovewrite = true, string prefix = null, string testsNamespace = null)
        {
            TemplatePath = templatePath;
            ProjItem = projItem;
            ParameterName = parameterName;
            ParameterCollection = parameterCollection;
            FolderPath = folderPath;
            FolderItem = folderItem;
            BinPath = binPath;
            TargetNamespace = targetNamespace;
            TestsNamespace = testsNamespace;
            Suffix = suffix;
            Ovewrite = ovewrite;
            Prefix = prefix;
        }

        public string TemplatePath { get; }

        public ProjectItem ProjItem { get; }

        public string ParameterName { get; }

        public IEnumerable<TT> ParameterCollection { get; }

        public string FolderPath { get; }

        public ProjectItem FolderItem { get; }

        public string BinPath { get; }

        public string TargetNamespace { get; }

        public string TestsNamespace { get; set; }

        public string Suffix { get; }

        public bool Ovewrite { get; }

        public string Prefix { get; }

        public string Title { get; set; }

        public bool IncludeHasModels { get; set; }

        public bool HasModels { get; set; }
        public bool UseAsyncMethods { get; set; }
        public bool IncludeApiVersionInRoutePrefix { get; set; }
        public string ApiVersion { get; set; }

        public string TargetFolder { get; set; }
        public string RelativeFolder { get; set; }
    }


}
