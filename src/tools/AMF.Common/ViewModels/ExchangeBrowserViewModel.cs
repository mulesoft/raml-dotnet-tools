using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;

namespace AMF.Common.ViewModels
{
    public class ExchangeBrowserViewModel : Screen
    {
        static HttpClient client = new HttpClient();
        private static readonly string BaseUrl = "https://anypoint.mulesoft.com";
        private readonly string AssetsUrl = BaseUrl + "/exchange/api/v1/assets";
        private readonly string DefaultQueryString = "type=rest-api&limit=" + AssetsLimit;
        private static readonly short AssetsLimit = 100;

        public string Title { get; set; }
        public string SearchText { get; set; }

        public ExchangeBrowserViewModel()
        {
            Title = "Select a REST API";
            Mouse.OverrideCursor = Cursors.Wait;
        }

        protected override void OnViewReady(object view)
        {
            GetProductsAsync().ConfigureAwait(false);
        }

        private ObservableCollection<ExchangeAsset> assets;
        public ObservableCollection<ExchangeAsset> Assets
        {
            get => assets;
            set
            {
                assets = value;
                NotifyOfPropertyChange(() => Assets);
            }
        }

        public ExchangeAsset SelectedAsset { get; set; }
        public void OnSelectionChangedAction(SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
                return;

            var selected = e.AddedItems[0] as ExchangeAsset;
            if (selected == null)
                return;

            ExchangeAsset selectedAsset = selected;
            SelectedAsset = selectedAsset;
            SelectEnabled = true;
        }

        private bool selectEnabled;
        private string accesToken;

        public bool SelectEnabled
        {
            get => selectEnabled;
            set
            {
                selectEnabled = value;
                NotifyOfPropertyChange(() => SelectEnabled);
            }
        }

        private async Task GetProductsAsync(string search = null)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var requestUri = AssetsUrl + "?" + DefaultQueryString;
            if (!string.IsNullOrWhiteSpace(search))
                requestUri += "&search=" + Uri.EscapeUriString(search);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                //requestUri += "&access_token=" + AccessToken;
                request.Headers.Add("Authorization", "Bearer " + AccessToken);
                // request.Headers.Add("Username/Password", "Authentication");
            }
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            Assets = new ObservableCollection<ExchangeAsset>(new JavaScriptSerializer().Deserialize<List<ExchangeAsset>>(content));
            Mouse.OverrideCursor = null;
        }

        public string Username { get; set; }
        public string AccessToken
        {
            get => accesToken; set
            {
                accesToken = value;
                NotifyOfPropertyChange(() => AccessToken);
                NotifyOfPropertyChange(() => IsLoggedIn);
                NotifyOfPropertyChange(() => IsNotLoggedIn);
            }
        }

        public bool IsLoggedIn
        {
            get => !string.IsNullOrWhiteSpace(AccessToken);
        }

        public bool IsNotLoggedIn
        {
            get => !IsLoggedIn;
        }

        public async void Login()
        {
            var loginViewModel = new LoginViewModel();
            WindowManager.ShowDialog(loginViewModel);

            if (loginViewModel.Token != null)
            {
                AccessToken = loginViewModel.Token;
                Username = loginViewModel.Username;
                await GetProductsAsync(SearchText); //reload with token
            }
        }

        public class ExchangeAsset
        {
            public string GroupId { get; set; }
            public string AssetId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Version { get; set; }
            public IEnumerable<File> Files { get; set; }
            public ExchangeOrganization Organization { get; set; }
            public string Link { get { return "https://anypoint.mulesoft.com/exchange/" + GroupId + "/" + AssetId; } }
            public string OrganizationName { get { return Organization?.Name; } }
        }

        public class ExchangeOrganization
        {
            public string Name { get; set; }
        }

        public class File
        {
            public string Classifier { get; set; } // ": "fat-raml / raml",
            public string Packaging { get; set; } // ": "zip",
            public string ExternalLink { get; set; } // ": "https://exchange2-asset-manager.s3.amazonaws.com/d83b3280-4ea8-4c3b-a8a2-ec6e589574f4/97a76ab9-3639-4002-9b84-bb6b751860d3/carlos-1.0.0-fat-raml.zip?AWSAccessKeyId=AKIAI5ZXWOH2ORZHJPTQ&Expires=1510926241&Signature=xOE%2BHrUE7ssYwYq259LQRdXQveY%3D&response-content-disposition=attachment%3B%20filename%3Dcarlos-1.0.0-raml.zip",
            public string MainFile { get; set; }
        }

        public async void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                return;

            await GetProductsAsync(SearchText);
        }

        public void Select()
        {
            if (SelectedAsset != null)
                TryClose();
        }

        public void Cancel()
        {
            SelectedAsset = null;
            TryClose();
        }

        private IWindowManager windowManager;

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

    }
}