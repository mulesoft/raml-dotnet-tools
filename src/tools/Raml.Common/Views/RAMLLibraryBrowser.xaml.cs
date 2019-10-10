using System;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using mshtml;
using Raml.Common.Properties;

namespace Raml.Common
{
    /// <summary>
    /// Interaction logic for RAMLLibraryBrowser.xaml
    /// </summary>
    public partial class RAMLLibraryBrowser : Window
    {
        private readonly string exchangeUrl;
        private const string PostData = "{\"perspective\":\"api\"}";
        private const string AdditionalHeaders = "User-Agent: studio\nX-Client-Id: vsnet1\nContent-Type: application/json";
        public string RAMLFileUrl { get; set; }

        public RAMLLibraryBrowser(string exchangeUrl)
        {
            this.exchangeUrl = exchangeUrl;
            InitializeComponent();
            var webEvents = new BrowserControlEvents(this);
            LibraryWebBrowser.ObjectForScripting = webEvents;
        }

        public void NewUrlSelected(string url)
        {
            RAMLFileUrl = url;
            DialogResult = true;
            Close();
        }

        private void NavigateToLibaryBrowser()
        {
            LibraryWebBrowser.Navigate(new Uri(exchangeUrl), null,
                Encoding.UTF8.GetBytes(PostData), AdditionalHeaders);
            
        }

        private void LibraryWebBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            var doc = LibraryWebBrowser.Document as HTMLDocument;
            var headElement = doc.getElementsByTagName("head").item(0);
            var scriptElement = doc.createElement("script");
            scriptElement.setAttribute("type", "text/javascript");
            var domElement = (IHTMLScriptElement)scriptElement;
            domElement.text = "function setRamlUrl(url) { window.external.SetRamlUrl(url); }";
            headElement.AppendChild(scriptElement);
        }

        private void RAMLLibraryBrowser_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigateToLibaryBrowser(); 
        }
    }
}

