using AMF.Common.ViewModels;

namespace AMF.Common.Views
{
    /// <summary>
    /// Interaction logic for RamlChooser.xaml
    /// </summary>
    public partial class RamlChooserView : IHavePassword
    {
        public RamlChooserView()
        {
            InitializeComponent();
            DataContext = new RamlChooserViewModel();
        }

        public System.Security.SecureString Password
        {
            get
            {
                return UserPassword.SecurePassword;
            }
        }
    }
}
