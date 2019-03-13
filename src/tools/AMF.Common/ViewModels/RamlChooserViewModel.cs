using System;
using System.Dynamic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Microsoft.Win32;
using AMF.Api.Core;
using System.Net.Http;
using System.Linq;
using System.IO.Compression;
using System.Security;
using System.Runtime.InteropServices;

namespace AMF.Common.ViewModels
{
    public class RamlChooserViewModel : Screen
    {
        private const string RamlFileExtension = ".yaml";

        // action to execute when clicking Ok button (add RAML Reference, Scaffold Web Api, etc.)
        private Action<RamlChooserActionParams> action;
        private string exchangeUrl;
        public string RamlTempFilePath { get; private set; }
        public string RamlOriginalSource { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        private bool isContractUseCase;
        private int height;
        private string url;
        private string title;
        private Visibility progressBarVisibility;
        private bool isNewRamlOption;
        private bool existingRamlOption;
        private IWindowManager windowManager;

        private string username;
        private bool enableBasicAuth;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                NotifyOfPropertyChange();
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private bool IsContractUseCase
        {
            get { return isContractUseCase; }
            set
            {
                if (value.Equals(isContractUseCase)) return;
                isContractUseCase = value;
                NotifyOfPropertyChange(() => ContractUseCaseVisibility);
            }
        }

        public Visibility ContractUseCaseVisibility
        {
            get
            {
                return IsContractUseCase ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Load(IServiceProvider serviceProvider, Action<RamlChooserActionParams> action, string title, bool isContractUseCase, string exchangeUrl)
        {
            this.action = action;
            this.exchangeUrl = exchangeUrl; // "https://qa.anypoint.mulesoft.com/exchange/#!/?types=api"; // testing URL
            ServiceProvider = serviceProvider;
            DisplayName = title;
            IsContractUseCase = isContractUseCase;
            Height = isContractUseCase ? 750 : 375;
            StopProgress();
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

        public bool CanAddNewContract
        {
            get { return !string.IsNullOrWhiteSpace(Title); }
        }

        public async void AddExistingRamlFromExchange()
        {
            var exchangeBrowseViewModel = new ExchangeBrowserViewModel();
            WindowManager.ShowDialog(exchangeBrowseViewModel);
            var selectedAsset = exchangeBrowseViewModel.SelectedAsset;
            if(selectedAsset != null)
            {
                var file = selectedAsset.Files.FirstOrDefault(f => f.Classifier == "fat-raml");
                if (file == null)
                    file = selectedAsset.Files.FirstOrDefault(f => f.Classifier == "raml");
                if (file == null)
                {
                    MessageBox.Show("The selected REST API does not seem to have any RAML file associated");
                    return;
                }

                var uri = file.ExternalLink;

                var client = new HttpClient();
                var byteArray = await client.GetByteArrayAsync(uri);
                var assetName = NetNamingMapper.GetObjectName(selectedAsset.Name);

                var zipPath = Path.Combine(Path.GetTempPath(), assetName + ".zip");
                File.WriteAllBytes(zipPath, byteArray);
                var destinationFolder = Path.Combine(Path.GetTempPath(), assetName + DateTime.Now.Ticks);
                ZipFile.ExtractToDirectory(zipPath, destinationFolder);

                RamlTempFilePath = GetRamlPath(destinationFolder, file.MainFile, uri);
                if (RamlTempFilePath == null)
                {
                    MessageBox.Show("Unable to determine main RAML file, please use the 'Upload' option to choose the right file from folder " + destinationFolder);
                    return;
                }

                var previewViewModel = new RamlPreviewViewModel(ServiceProvider, action, RamlTempFilePath, RamlTempFilePath,
                    Path.GetFileName(RamlTempFilePath), isContractUseCase, useBasicAuth: false, username: string.Empty, password: string.Empty);

                try
                {
                    StartProgress();
                    await previewViewModel.FromFile();
                }
                finally
                {
                    StopProgress();
                }

                ShowPreviewViewAndClose(previewViewModel);
            }
        }

        public string GetRamlPath(string destinationFolder, string mainFile, string uri)
        {
            if (!string.IsNullOrWhiteSpace(mainFile))
            {
                var path = Path.Combine(destinationFolder, mainFile);
                if(File.Exists(path))
                    return path;
            }

            var fileName = Path.GetFileName(uri);
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var path = Path.Combine(destinationFolder, fileName.Replace(".zip", string.Empty));
                if(File.Exists(path))
                    return path;
            }

            var files = Directory.GetFiles(destinationFolder, "*.raml");
            if(files.Count() == 1 && File.Exists(files[0]))
                return files[0];

            var defaultPath = Path.Combine(destinationFolder, "api.raml");
            if (File.Exists(defaultPath))
                return defaultPath;

            return null;
        }

        public async void AddExistingRamlFromDisk()
        {
            SelectExistingRamlOption();
            FileDialog fd = new OpenFileDialog();
            fd.DefaultExt = ".raml;*.rml;*.yaml;*.json";
            fd.Filter = "RAML/OAS files |*.raml;*.rml;*.yaml;*.json";

            var opened = fd.ShowDialog();

            if (opened != true)
            {
                return;
            }

            RamlTempFilePath = fd.FileName;
            RamlOriginalSource = fd.FileName;

            var previewViewModel = new RamlPreviewViewModel(ServiceProvider, action, RamlTempFilePath, RamlOriginalSource,
                Path.GetFileName(fd.FileName), isContractUseCase, useBasicAuth: false, username: string.Empty, password: string.Empty);

            try
            {
                StartProgress();
                await previewViewModel.FromFile();
            }
            finally
            {
                StopProgress();
            }

            ShowPreviewViewAndClose(previewViewModel);
        }

        private IWindowManager WindowManager
        {
            get
            {
                if (windowManager != null) 
                    return windowManager;
                
                try
                {
                    windowManager = IoC.Get<IWindowManager>();
                }
                catch
                {
                    windowManager = new WindowManager();
                }
                return windowManager;
            }
        }

        private void ShowPreviewViewAndClose(RamlPreviewViewModel previewViewModel)
        {
            dynamic settings = new ExpandoObject();
            settings.Height = isContractUseCase ? 680 : 520;
            WindowManager.ShowDialog(previewViewModel, null, settings);

            if (previewViewModel.WasImported)
                TryClose();
        }

        private void StartProgress()
        {
            ProgressBarVisibility = Visibility.Visible;
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

        private void StopProgress()
        {
            ProgressBarVisibility = Visibility.Hidden;
            Mouse.OverrideCursor = null;
        }

        //public async void AddExistingRamlFromExchange()
        //{
        //    SelectExistingRamlOption();
        //    var rmlLibrary = new RAMLLibraryBrowser(exchangeUrl);
        //    var selectedRamlFile = rmlLibrary.ShowDialog();

        //    if (selectedRamlFile.HasValue && selectedRamlFile.Value)
        //    {
        //        var url = rmlLibrary.RAMLFileUrl;

        //        Url = url;

        //        var previewViewModel = new RamlPreviewViewModel(ServiceProvider, action, RamlTempFilePath, url, "title", isContractUseCase);
                
        //        StartProgress();
        //        await previewViewModel.FromUrl();
        //        StopProgress();

        //        ShowPreviewViewAndClose(previewViewModel);
        //    }
        //}

        public string Url
        {
            get { return url; }
            set
            {
                if (value == url) return;
                url = value;
                NotifyOfPropertyChange(() => Url);
                NotifyOfPropertyChange(() => CanAddExistingRamlFromUrl);
            }
        }


        public async void AddExistingRamlFromUrl(object parameter)
        {
            SelectExistingRamlOption();
            var password = string.Empty;
            if (EnableBasicAuth)
            {
                if (string.IsNullOrWhiteSpace(Username))
                {
                    MessageBox.Show("Please type your username.");
                    return;
                }
                if (parameter is IHavePassword passwordContainer)
                {
                    var secureString = passwordContainer.Password;
                    password = ConvertToUnsecureString(secureString);
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please type your password.");
                    return;
                }
            }

            var previewViewModel = new RamlPreviewViewModel(ServiceProvider, action, RamlTempFilePath, Url, "title", isContractUseCase, EnableBasicAuth, Username, password);

            try
            {
                StartProgress();
                await previewViewModel.FromUrl();
            }
            finally
            {
                StopProgress();
            }
            
            ShowPreviewViewAndClose(previewViewModel);
        }

        public bool IsNewRamlOption
        {
            get { return isNewRamlOption; }
            set
            {
                if (value == isNewRamlOption) return;
                isNewRamlOption = value;
                NotifyOfPropertyChange(() => IsNewRamlOption);
            }
        }

        public bool CanAddExistingRamlFromUrl
        {
            get { return !string.IsNullOrWhiteSpace(Url); }
        }

        public bool ExistingRamlOption
        {
            get { return existingRamlOption; }
            set
            {
                if (value == existingRamlOption) return;
                existingRamlOption = value;
                NotifyOfPropertyChange(() => ExistingRamlOption);
            }
        }

        public void Url_Changed()
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            SelectExistingRamlOption();
        }

        public void Title_Changed()
        {
            if (string.IsNullOrWhiteSpace(title)) 
                return;

            SelectNewRamlOption();
            NewRamlFilename = NetNamingMapper.RemoveIndalidChars(Title) + RamlFileExtension;
            NewRamlNamespace = GetNamespace(NewRamlFilename);
        }

        public string Title
        {
            get { return title; }
            set
            {
                if (value == title) return;
                title = value;
                NotifyOfPropertyChange(() => Title);
                NotifyOfPropertyChange(() => CanAddNewContract);
            }
        }

        public string NewRamlNamespace { get; set; }

        public string NewRamlFilename { get; set; }

        public bool EnableBasicAuth
        {
            get { return enableBasicAuth; }
            set
            {
                if (value == enableBasicAuth) return;
                enableBasicAuth = value;
                NotifyOfPropertyChange();
            }
        }

        private string GetNamespace(string fileName)
        {
            return VisualStudioAutomationHelper.GetDefaultNamespace(ServiceProvider) + "." +
                     NetNamingMapper.GetObjectName(Path.GetFileNameWithoutExtension(fileName));
        }

        private void SelectExistingRamlOption()
        {
            ExistingRamlOption = true;
            IsNewRamlOption = false;
        }

        private void SelectNewRamlOption()
        {
            IsNewRamlOption = true;
            ExistingRamlOption = false;
        }

        public void Cancel()
        {
            StopProgress();
            TryClose();
        }

        public void AddNewContract()
        {
            var previewViewModel = new RamlPreviewViewModel(ServiceProvider, action, Title, useBasicAuth: false, username: string.Empty, 
                password: string.Empty);
            previewViewModel.NewContract();
            dynamic settings = new ExpandoObject();
            settings.Height = 420;
            WindowManager.ShowDialog(previewViewModel, null, settings);
            TryClose();
        }
    }
}