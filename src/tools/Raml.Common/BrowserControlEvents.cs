using System.Security.Permissions;

namespace Raml.Common
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class BrowserControlEvents
    {
        readonly RAMLLibraryBrowser _externalWpf;
        public BrowserControlEvents(RAMLLibraryBrowser w)
        {
            _externalWpf = w;
        }

        public void SetRamlUrl(string url)
        {
            _externalWpf.NewUrlSelected(url);
        }
    }
}