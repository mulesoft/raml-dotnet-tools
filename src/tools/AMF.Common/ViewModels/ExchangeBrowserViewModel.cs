using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Controls;
using Caliburn.Micro;
using Newtonsoft.Json;

namespace AMF.Common.ViewModels
{
    public class ExchangeBrowserViewModel : Screen
    {
        static HttpClient client = new HttpClient();
        private readonly string url = "https://anypoint.mulesoft.com/exchange/api/v1/assets";

        public string Title { get; set; }

        public ExchangeBrowserViewModel()
        {
            Title = "Select a REST API";
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
        public void OnSelectionChangedAction(object sender, SelectionChangedEventArgs e, object dataContext)
        {
            if (e.AddedItems.Count != 1)
                return;

            var selected =  e.AddedItems[0] as ExchangeAsset;
            if (selected == null)
                return;

            ExchangeAsset selectedAsset = selected;
            SelectedAsset = selectedAsset;
            SelectEnabled = true;
        }

        private bool selectEnabled;
        public bool SelectEnabled
        {
            get => selectEnabled;
            set
            {
                selectEnabled = value;
                NotifyOfPropertyChange(() => SelectEnabled);
            }
        }

        private async Task GetProductsAsync()
        {
            var response = await client.GetAsync(url + "?type=rest-api");
            var content = await response.Content.ReadAsStringAsync();
            Assets = new ObservableCollection<ExchangeAsset>(new JavaScriptSerializer().Deserialize<List<ExchangeAsset>>(content));
        }

        public class ExchangeAsset
        {
            public string GroupId { get; set; }
            public string AssetId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public IEnumerable<File> Files { get; set; }
        }

        public class File
        {
            public string Classifier { get; set; } // ": "fat-raml / raml",
            public string Packaging { get; set; } // ": "zip",
            public string ExternalLink { get; set; } // ": "https://exchange2-asset-manager.s3.amazonaws.com/d83b3280-4ea8-4c3b-a8a2-ec6e589574f4/97a76ab9-3639-4002-9b84-bb6b751860d3/carlos-1.0.0-fat-raml.zip?AWSAccessKeyId=AKIAI5ZXWOH2ORZHJPTQ&Expires=1510926241&Signature=xOE%2BHrUE7ssYwYq259LQRdXQveY%3D&response-content-disposition=attachment%3B%20filename%3Dcarlos-1.0.0-raml.zip",
            public string MainFile { get; set; }
        }

        public void Select()
        {
            if(SelectedAsset != null)
                TryClose();
        }

        public void Cancel()
        {
            SelectedAsset = null;
            TryClose();
        }
    }
}