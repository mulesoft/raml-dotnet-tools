using System.Collections.Generic;
using EnvDTE;
using Raml.Tools.WebApiGenerator;

namespace MuleSoft.RAML.Tools
{
    public class TemplateParams<TT> where TT : IHasName
    {
        private string _templatePath;
        private ProjectItem _projItem;
        private string _parameterName;
        private IEnumerable<TT> _parameterCollection;
        private string _folderPath;
        private ProjectItem _folderItem;
        private string _binPath;
        private string _targetNamespace;
        private string _suffix;
        private bool _ovewrite;
        private string _prefix;

        public TemplateParams(string templatePath, ProjectItem projItem, string parameterName,
            IEnumerable<TT> parameterCollection, string folderPath, ProjectItem folderItem, string binPath,
            string targetNamespace, string suffix = null, bool ovewrite = true, string prefix = null)
        {
            _templatePath = templatePath;
            _projItem = projItem;
            _parameterName = parameterName;
            _parameterCollection = parameterCollection;
            _folderPath = folderPath;
            _folderItem = folderItem;
            _binPath = binPath;
            _targetNamespace = targetNamespace;
            _suffix = suffix;
            _ovewrite = ovewrite;
            _prefix = prefix;
        }

        public string TemplatePath
        {
            get { return _templatePath; }
        }

        public ProjectItem ProjItem
        {
            get { return _projItem; }
        }

        public string ParameterName
        {
            get { return _parameterName; }
        }

        public IEnumerable<TT> ParameterCollection
        {
            get { return _parameterCollection; }
        }

        public string FolderPath
        {
            get { return _folderPath; }
        }

        public ProjectItem FolderItem
        {
            get { return _folderItem; }
        }

        public string BinPath
        {
            get { return _binPath; }
        }

        public string TargetNamespace
        {
            get { return _targetNamespace; }
        }

        public string Suffix
        {
            get { return _suffix; }
        }

        public bool Ovewrite
        {
            get { return _ovewrite; }
        }

        public string Prefix
        {
            get { return _prefix; }
        }

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
