using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using AMF.Parser;
using AMF.Parser.Model;
using System.Linq;
using Task = System.Threading.Tasks.Task;
using AMF.Api.Core;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;

namespace AMF.Common.ViewModels
{
    public class RamlPreviewViewModel : Screen
    {
        private const string RamlFileExtension = ".raml";
        private readonly RamlIncludesManager includesManager = new RamlIncludesManager();
        // action to execute when clicking Ok button (add RAML Reference, Scaffold Web Api, etc.)
        private readonly Action<RamlChooserActionParams> action;
        private readonly bool isNewContract;
        private bool isContractUseCase;
        private bool useApiVersion;
        private bool configFolders;
        private string modelsFolder;
        private string activeProjectName;
        private Logger logger = new Logger();

        public RamlPreviewViewModel(IServiceProvider serviceProvider, Action<RamlChooserActionParams> action, string ramlTempFilePath,
            string ramlOriginalSource, string ramlTitle, bool isContractUseCase)
            : this(serviceProvider, ramlTitle)
        {
            DisplayName = "Import RAML";
            ImportButtonText = "Import";
            RamlTempFilePath = ramlTempFilePath;
            RamlOriginalSource = ramlOriginalSource;
            IsContractUseCase = isContractUseCase;
            this.action = action;
            Height = isContractUseCase ? 660 : 480;
        }

        public RamlPreviewViewModel(IServiceProvider serviceProvider, Action<RamlChooserActionParams> action, string ramlTitle)
            : this(serviceProvider, ramlTitle)
        {
            DisplayName = "Create New RAML Contract";
            ImportButtonText = "Create";
            IsContractUseCase = true;
            this.action = action;
            isNewContract = true;
            Height = 420;
            CanImport = true;
            NotifyOfPropertyChange(() => NewContractVisibility);
        }

        private RamlPreviewViewModel(IServiceProvider serviceProvider, string ramlTitle)
        {
            ServiceProvider = serviceProvider;
            RamlTitle = ramlTitle;
            var dte = serviceProvider.GetService(typeof(SDTE)) as DTE;
            Projects = VisualStudioAutomationHelper.GetProjects(dte);
            activeProjectName = VisualStudioAutomationHelper.GetActiveProject(dte).Name;
        }

        public string ImportButtonText
        {
            get { return importButtonText; }
            set
            {
                if (value == importButtonText) return;
                importButtonText = value;
                NotifyOfPropertyChange(() => ImportButtonText);
            }
        }

        public string RamlTempFilePath { get; private set; }
        public string RamlOriginalSource { get; private set; }
        public string RamlTitle { get; private set; }

        public IServiceProvider ServiceProvider { get; set; }

        private bool IsContractUseCase
        {
            get { return isContractUseCase; }
            set
            {
                isContractUseCase = value;
                NotifyOfPropertyChange(() => ClientUseCaseVisibility);
                NotifyOfPropertyChange(() => ContractUseCaseVisibility);
            }
        }

        public Visibility ClientUseCaseVisibility { get { return isContractUseCase ? Visibility.Collapsed : Visibility.Visible; } }
        public Visibility ContractUseCaseVisibility { get { return isContractUseCase ? Visibility.Visible : Visibility.Collapsed; } }

        public Visibility NewContractVisibility { get { return isNewContract ? Visibility.Collapsed : Visibility.Visible; } }


        public bool UseApiVersion
        {
            get { return useApiVersion; }
            set
            {
                useApiVersion = value;

                // Set a default value if version not specified
                if (useApiVersion && string.IsNullOrWhiteSpace(ApiVersion))
                    ApiVersion = "v1";

                ApiVersionIsEnabled = useApiVersion;
            }
        }

        public bool ApiVersionIsEnabled
        {
            get { return apiVersionIsEnabled; }
            set
            {
                if (value == apiVersionIsEnabled) return;
                apiVersionIsEnabled = value;
                NotifyOfPropertyChange(() => ApiVersionIsEnabled);
            }
        }

        public string ApiVersion
        {
            get { return apiVersion; }
            set
            {
                if (value == apiVersion) return;
                apiVersion = value;
                NotifyOfPropertyChange(() => ApiVersion);
            }
        }

        private bool generateUnitTests;

        public bool GenerateUnitTests
        {
            get { return generateUnitTests; }
            set
            {
                generateUnitTests = value;
                NotifyOfPropertyChange(() => GenerateUnitTests);
            }
        }


        public bool CustomizeFoldersAndNamespaces
        {
            get { return configFolders; }
            set
            {
                configFolders = value;
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

        public string ModelsNamespace
        {
            get { return modelsNamespace; }
            set
            {
                modelsNamespace = value;
                NotifyOfPropertyChange();
            }
        }

        public string ControllersNamespace
        {
            get { return controllersNamespace; }
            set
            {
                controllersNamespace = value;
                NotifyOfPropertyChange();
            }
        }


        private string implementationControllersFolder;
        public string ImplementationControllersFolder
        {
            get { return implementationControllersFolder; }
            set
            {
                implementationControllersFolder = value;
                NotifyOfPropertyChange();
            }
        }

        protected override void OnViewReady(object view)
        {
            if (!isNewContract)
            {
                //StartProgress();
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (value == height) return;
                height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        private AmfModel _model;

        private void SetPreview(AmfModel model)
        {
            _model = model;
            var document = model.WebApi;

            Execute.OnUIThreadAsync(() =>
            {
                try
                {
                    StartProgress();
                    ResourcesPreview = GetResourcesPreview(document);
                    StopProgress();
                    SetDefaultNamespaces(RamlTempFilePath);
                    if (document.Version != null)
                        ApiVersion = NetNamingMapper.GetVersionName(document.Version);
                    CanImport = true;

                    if (NetNamingMapper.HasIndalidChars(Filename))
                    {
                        ShowErrorAndStopProgress("The specied file name has invalid chars");
                        // txtFileName.Focus();
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorAndStopProgress("Error while parsing raml file. " + ex.Message);
                    logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, VisualStudioAutomationHelper.GetExceptionInfo(ex));
                }
                finally
                {
                    StopProgress();
                }
            });
        }

        public string Filename
        {
            get { return filename; }
            set
            {
                if (value == filename) return;
                filename = value;
                NotifyOfPropertyChange(() => Filename);
            }
        }

        public bool CanImport
        {
            get { return canImport; }
            set
            {
                if (value == canImport) return;
                canImport = value;
                NotifyOfPropertyChange(() => CanImport);
            }
        }

        public string ResourcesPreview
        {
            get { return resourcesPreview; }
            set
            {
                if (value == resourcesPreview) return;
                resourcesPreview = value;
                NotifyOfPropertyChange(() => ResourcesPreview);
            }
        }

        private IEnumerable<string> projects;
        public IEnumerable<string> Projects
        {
            get => projects;
            set {
                if (value == projects) return;
                projects = value;
                NotifyOfPropertyChange(() => Projects);
            }
        }

        public string SelectedProject
        {
            get { return testsProject; }
            set
            {
                if (value == testsProject) return;
                testsProject = value;
                NotifyOfPropertyChange(() => SelectedProject);
            }
        }

        public string TestsNamespace
        {
            get { return testsNamespace; }
            set
            {
                if (value == testsNamespace) return;
                testsNamespace = value;
                NotifyOfPropertyChange(() => TestsNamespace);
            }
        }

        public string Namespace
        {
            get { return ns; }
            set
            {
                if (value == ns) return;
                ns = value;
                NotifyOfPropertyChange(() => Namespace);
            }
        }

        private void SetDefaultNamespaces(string fileName)
        {
            Namespace = VisualStudioAutomationHelper.GetDefaultNamespace(ServiceProvider) + "." +
                        NetNamingMapper.GetObjectName(Path.GetFileNameWithoutExtension(fileName));
            ControllersNamespace = Namespace;
            ModelsNamespace = Namespace + ".Models";
        }

        private static string GetResourcesPreview(WebApi ramlDoc)
        {
            return GetChildResources(ramlDoc.EndPoints, 0);
        }

        const int IndentationSpaces = 4;
        private static string GetChildResources(IEnumerable<EndPoint> resources, int level)
        {
            var output = string.Empty;
            foreach (var childResource in resources)
            {
                
                output += new string(' ', level * IndentationSpaces) + childResource.Path;
                //if (childResource.Resources.Any())
                //{
                //    output += Environment.NewLine;
                //    output += GetChildResources(childResource.Resources, level + 1);
                //}
                //else
                //{
                    output += Environment.NewLine;
                //}
            }
            return output;
        }

        private void StartProgress()
        {
            ProgressBarVisibility = Visibility.Visible;
            CanImport = false;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public Visibility ProgressBarVisibility
        {
            get { return progressBarVisibility; }
            set
            {
                if (value == progressBarVisibility) return;
                progressBarVisibility = value;
                NotifyOfPropertyChange(() => ProgressBarVisibility);
            }
        }

        private void ShowErrorAndStopProgress(string errorMessage)
        {
            if (!isNewContract)
                ResourcesPreview = errorMessage;
            else
                MessageBox.Show(errorMessage);
                
            
            StopProgress();
        }

        private void StopProgress()
        {
            ProgressBarVisibility = Visibility.Hidden;
            Mouse.OverrideCursor = null;
        }

        private async Task GetRamlFromUrl()
        {
            //StartProgress();
            //DoEvents();

            try
            {
                var url = RamlOriginalSource;
                var result = includesManager.Manage(url, Path.GetTempPath(), Path.GetTempPath());

                var raml = result.ModifiedContents;
                var parser = new AmfParser();

                var tempPath = Path.GetTempFileName();
                File.WriteAllText(tempPath, raml);

                var amfModel = await parser.Load(tempPath);

                SetFilename(url);

                var path = Path.Combine(Path.GetTempPath(), Filename);
                File.WriteAllText(path, raml);
                RamlTempFilePath = path;
                RamlOriginalSource = url;

                SetPreview(amfModel);

                //CanImport = true;
                //StopProgress();
            }
            catch (UriFormatException uex)
            {
                ShowErrorAndDisableOk(uex.Message);
            }
            catch (HttpRequestException rex)
            {
                ShowErrorAndDisableOk(GetFriendlyMessage(rex));
                logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(rex));
            }
            catch (Exception ex)
            {
                ShowErrorAndDisableOk(ex.Message);
                logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(ex));
            }
        }

        private void SetFilename(string url)
        {
            Filename = GetFilename(url);
        }

        private static string GetFilename(string url)
        {
            var filename = Path.GetFileName(url);

            if (string.IsNullOrEmpty(filename))
                filename = "reference.yaml";

            //if (!filename.ToLowerInvariant().EndsWith(RamlFileExtension))
            //    filename += RamlFileExtension;

            filename = NetNamingMapper.RemoveIndalidChars(Path.GetFileNameWithoutExtension(filename)) +
                       Path.GetExtension(filename);

            return filename;
        }

        private static string GetFriendlyMessage(HttpRequestException rex)
        {
            if (rex.Message.Contains("404"))
                return "Could not find specified URL. Server responded with Not Found (404) status code";

            return rex.Message;
        }

        public async Task Import()
        {
            try
            {
                StartProgress();
                // DoEvents();

                if (string.IsNullOrWhiteSpace(Namespace))
                {
                    ShowErrorAndStopProgress("Error: you must specify a namespace.");
                    return;
                }

                if (IsContractUseCase && CustomizeFoldersAndNamespaces && string.IsNullOrWhiteSpace(ControllersNamespace))
                {
                    ShowErrorAndStopProgress("Error: you must specify a namespace for controllers.");
                    return;
                }

                if (IsContractUseCase && CustomizeFoldersAndNamespaces && string.IsNullOrWhiteSpace(ModelsNamespace))
                {
                    ShowErrorAndStopProgress("Error: you must specify a namespace for models.");
                    return;
                }

                if (IsContractUseCase && GenerateUnitTests && string.IsNullOrWhiteSpace(SelectedProject))
                {
                    ShowErrorAndStopProgress("Error: you must select a project to generate the unit tests.");
                    return;
                }

                if (IsContractUseCase && GenerateUnitTests && SelectedProject == activeProjectName)
                {
                    ShowErrorAndStopProgress("Error: select a different project to generate the unit tests.");
                    return;
                }

                if (!IsContractUseCase && !File.Exists(RamlTempFilePath))
                {
                    ShowErrorAndStopProgress("Error: the specified file does not exist.");
                    return;
                }

                if (IsContractUseCase && UseApiVersion && string.IsNullOrWhiteSpace(ApiVersion))
                {
                    ShowErrorAndStopProgress("Error: you need to specify a version.");
                    return;
                }

                if (IsContractUseCase && CustomizeFoldersAndNamespaces && HasInvalidPath(ModelsFolder))
                {
                    ShowErrorAndStopProgress("Error: invalid path specified for models. Path must be relative.");
                    //txtModels.Focus();
                    return;
                }

                if (IsContractUseCase && CustomizeFoldersAndNamespaces && HasInvalidPath(ImplementationControllersFolder))
                {
                    ShowErrorAndStopProgress("Error: invalid path specified for controllers. Path must be relative.");
                    //txtImplementationControllers.Focus();
                    return;
                }

            }
            finally
            {
                StopProgress();
            }

            var path = Path.GetDirectoryName(GetType().Assembly.Location) + Path.DirectorySeparatorChar;

            try
            {
                ResourcesPreview = "Processing. Please wait..." + Environment.NewLine + Environment.NewLine;

                // Execute action (add RAML Reference, Scaffold Web Api, etc)
                var parameters = new RamlChooserActionParams(RamlOriginalSource, RamlTempFilePath, RamlTitle, path, Filename, Namespace, 
                    doNotScaffold: isNewContract);

                if (isContractUseCase)
                {
                    parameters.UseAsyncMethods = UseAsyncMethods;
                    parameters.IncludeApiVersionInRoutePrefix = UseApiVersion;
                    parameters.ImplementationControllersFolder = GetCorrectPath(ImplementationControllersFolder);
                    parameters.ModelsFolder = GetCorrectPath(ModelsFolder);
                    parameters.AddGeneratedSuffixToFiles = AddSuffixToGeneratedCode;
                    parameters.GenerateUnitTests = GenerateUnitTests;
                    parameters.TestsProjectName = SelectedProject;
                    parameters.TestsNamespace = TestsNamespace;
                    if (!CustomizeFoldersAndNamespaces)
                    {
                        ControllersNamespace = Namespace;
                        ModelsNamespace = Namespace + ".Models";
                    }
                    parameters.ModelsNamespace = ModelsNamespace;
                    parameters.ControllersNamespace = ControllersNamespace;
                }

                if(!isContractUseCase)
                    parameters.ClientRootClassName = ProxyClientName;

                if (!isNewContract)
                {
                    var ramlInfo = await RamlInfoService.GetRamlInfo(parameters.RamlFilePath, _model);
                    parameters.Data = ramlInfo;
                }
                action(parameters);

                ResourcesPreview += "Succeeded";
                StopProgress();
                CanImport = true;
                WasImported = true;
                TryClose();
            }
            catch (Exception ex)
            {
                ShowErrorAndStopProgress("Error: " + ex.Message);

                logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource, VisualStudioAutomationHelper.GetExceptionInfo(ex));
            }
        }

        private string GetCorrectPath(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                return folder;

            return folder.TrimStart('/', '\\');
        }

        public bool WasImported { get; set; }

        public string ProxyClientName
        {
            get { return proxyClientName; }
            set
            {
                if (value == proxyClientName) return;
                proxyClientName = value;
                NotifyOfPropertyChange(() => ProxyClientName);
            }
        }

        public bool AddSuffixToGeneratedCode
        {
            get { return addSuffixToGeneratedCode; }
            set
            {
                if (value == addSuffixToGeneratedCode) return;
                addSuffixToGeneratedCode = value;
                NotifyOfPropertyChange(() => AddSuffixToGeneratedCode);
            }
        }

        public bool UseAsyncMethods { get; set; }
        

        private readonly char[] invalidPathChars = Path.GetInvalidPathChars().Union((new[] {':'}).ToList()).ToArray();
        private int height;
        private string apiVersion;
        private bool apiVersionIsEnabled;
        private string resourcesPreview;
        private bool canImport;
        private string filename;
        private string ns;
        private string testsNamespace;
        private string testsProject;
        private Visibility progressBarVisibility;
        private bool addSuffixToGeneratedCode;
        private string proxyClientName;
        private string importButtonText;
        private string modelsNamespace;
        private string controllersNamespace;

        private bool HasInvalidPath(string folder)
        {
            if (folder == null)
                return false;

            return invalidPathChars.Any(folder.Contains);
        }

        public void Cancel()
        {
            WasImported = false;
            StopProgress();
            TryClose();
        }

        private void ShowErrorAndDisableOk(string errorMessage)
        {
            ShowError(errorMessage);
            CanImport = false;
        }

        private void ShowError(string errorMessage)
        {
            ResourcesPreview = errorMessage;
        }



        //#region refresh UI
        //[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        //public void DoEvents()
        //{
            
        //    Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);

        //    //DispatcherFrame frame = new DispatcherFrame();
        //    //Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
        //    //    new DispatcherOperationCallback(ExitFrame), frame);
        //    //Dispatcher.PushFrame(frame);
        //}

        //public object ExitFrame(object f)
        //{
        //    ((DispatcherFrame)f).Continue = false;

        //    return null;
        //}
        //#endregion

        public void NewContract()
        {
            SetFilename(RamlTitle + RamlFileExtension);
            SetDefaultNamespaces(Filename);
        }

        public async Task FromFile()
        {
            try
            {
                Filename = Path.GetFileName(RamlTempFilePath);

                SetDefaultClientRootClassName();

                var result = includesManager.Manage(RamlTempFilePath, Path.GetTempPath(), Path.GetTempPath());
                var parser = new AmfParser();

                var tempPath = Path.GetTempFileName();
                File.WriteAllText(tempPath, result.ModifiedContents);

                var amfModel = await parser.Load(tempPath);

                SetPreview(amfModel);
            }
            catch (Exception ex)
            {
                ShowErrorAndStopProgress("Error while parsing raml file. " + ex.Message);
                logger.LogError(VisualStudioAutomationHelper.RamlVsToolsActivityLogSource,
                    VisualStudioAutomationHelper.GetExceptionInfo(ex));
            }
        }

        private void SetDefaultClientRootClassName()
        {
            var rootName = NetNamingMapper.GetObjectName(Path.GetFileNameWithoutExtension(RamlTempFilePath));
            if (!rootName.ToLower().Contains("client"))
                rootName += "Client";
            ProxyClientName = rootName;
        }

        public async Task FromUrl()
        {
            SetDefaultClientRootClassName();
            await GetRamlFromUrl();
        }
         
    }
}