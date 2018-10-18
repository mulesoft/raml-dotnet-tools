using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace AMF.Common.Views
{
    /// <summary>
    /// Interaction logic for ExchangeBrowserView.xaml
    /// </summary>
    public partial class ExchangeBrowserView
    {
        public ExchangeBrowserView()
        {
            InitializeComponent();
        }

        private void HandleLinkClick(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            if(link.NavigateUri != null)
                Process.Start(link.NavigateUri.AbsoluteUri);
            e.Handled = true;
        }

    }
}
