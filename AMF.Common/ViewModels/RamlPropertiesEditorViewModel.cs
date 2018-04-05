using System.IO;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using Raml.Common;

namespace AMF.Common.ViewModels
{
    public class RamlPropertiesEditorViewModel : Screen
    {
        private bool isServerUseCase;

        private string ramlPath;
        private string ns;
        private string source;
        private string clientName;
        private bool useAsyncMethods;
        private bool includeApiVersionInRoutePrefix;
        private string baseControllersFolder;
        private string implementationControllersFolder;
        private string modelsFolder;
        private bool addGeneratedSuffixToFiles;

        public RamlPropertiesEditorViewModel()
        {
            DisplayName = "Edit RAML Properties";
        }

        public string Namespace
        {
            get { return ns; }
            set
            {
                ns = value;
                NotifyOfPropertyChange();
            }
        }

        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                NotifyOfPropertyChange();
            }
        }

        public string ClientName
        {
            get { return clientName; }
            set
            {
                clientName = value;
                NotifyOfPropertyChange();
            }
        }

        public bool UseAsyncMethods
        {
            get { return useAsyncMethods; }
            set
            {
                useAsyncMethods = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IncludeApiVersionInRoutePrefix
        {
            get { return includeApiVersionInRoutePrefix; }
            set
            {
                includeApiVersionInRoutePrefix = value;
                NotifyOfPropertyChange();
            }
        }

        public string ModelsFolder
        {
            get { return modelsFolder; }
            set
            {
                modelsFolder = value;
                NotifyOfPropertyChange();
            }
        }

        public string BaseControllersFolder
        {
            get { return baseControllersFolder; }
            set
            {
                baseControllersFolder = value;
                NotifyOfPropertyChange();
            }
        }

        public string ImplementationControllersFolder
        {
            get { return implementationControllersFolder; }
            set
            {
                implementationControllersFolder = value;
                NotifyOfPropertyChange();
            }
        }

        public bool WasSaved { get; set; }

        public Visibility ServerIsVisible
        {
            get { return isServerUseCase ? Visibility.Visible : Visibility.Collapsed; }
        }

        public Visibility ClientIsVisible
        {
            get { return isServerUseCase ? Visibility.Collapsed : Visibility.Visible; }
        }

        public bool AddGeneratedSuffixToFiles
        {
            get { return addGeneratedSuffixToFiles; }
            set
            {
                addGeneratedSuffixToFiles = value;
                NotifyOfPropertyChange();
            }
        }

        public void Load(string ramlPathParam, string serverPath, string clientPath)
        {
            ramlPath = ramlPathParam;
            if (ramlPathParam.Contains(serverPath) && !ramlPathParam.Contains(clientPath))
                isServerUseCase = true;

            var ramlProperties = RamlPropertiesManager.Load(ramlPathParam);
            Namespace = ramlProperties.Namespace;
            Source = ramlProperties.Source;
            if (isServerUseCase)
            {
                UseAsyncMethods = ramlProperties.UseAsyncMethods.HasValue && ramlProperties.UseAsyncMethods.Value;
                IncludeApiVersionInRoutePrefix = ramlProperties.IncludeApiVersionInRoutePrefix.HasValue &&
                                                 ramlProperties.IncludeApiVersionInRoutePrefix.Value;
                ModelsFolder = ramlProperties.ModelsFolder;
                AddGeneratedSuffixToFiles = ramlProperties.AddGeneratedSuffix != null && ramlProperties.AddGeneratedSuffix.Value;
                ImplementationControllersFolder = ramlProperties.ImplementationControllersFolder;
            }
            else
                ClientName = ramlProperties.ClientName;

            NotifyOfPropertyChange(() => ServerIsVisible);
            NotifyOfPropertyChange(() => ClientIsVisible);
        }

        public void SaveButton()
        {
            if (Namespace == null || NetNamingMapper.HasIndalidChars(Namespace))
            {
                MessageBox.Show("Error: invalid namespace.");
                return;
            }
            if (clientName != null && NetNamingMapper.HasIndalidChars(ClientName))
            {
                MessageBox.Show("Error: invalid client name.");
                return;
            }
            if (source != null && NetNamingMapper.HasIndalidChars(source))
            {
                MessageBox.Show("Error: invalid source.");
                return;
            }
            if (HasInvalidPath(ModelsFolder))
            {
                MessageBox.Show("Error: invalid path specified for models. Path must be relative.");
                return;
            }

            if (HasInvalidPath(ImplementationControllersFolder))
            {
                MessageBox.Show("Error: invalid path specified for controllers. Path must be relative.");
                return;
            }


            var ramlProperties = new RamlProperties
            {
                Namespace = Namespace,
                Source = Source,
                ClientName = ClientName,
                UseAsyncMethods = UseAsyncMethods,
                IncludeApiVersionInRoutePrefix = IncludeApiVersionInRoutePrefix,
                ModelsFolder = ModelsFolder,
                AddGeneratedSuffix = AddGeneratedSuffixToFiles,
                ImplementationControllersFolder = ImplementationControllersFolder
            };

            RamlPropertiesManager.Save(ramlProperties, ramlPath);
            WasSaved = true;
            TryClose();
        }

        public void CancelButton()
        {
            WasSaved = false;
            TryClose();
        }

        private readonly char[] invalidPathChars = Path.GetInvalidPathChars().Union((new[] { ':' }).ToList()).ToArray();
        private bool HasInvalidPath(string folder)
        {
            if (folder == null)
                return false;

            return invalidPathChars.Any(folder.Contains);
        }
    }
}