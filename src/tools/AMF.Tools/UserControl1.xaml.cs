using AMF.Parser;
using System.Windows;

namespace AMF.Tools
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : BaseDialogWindow
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var parser = new AmfParser();
            var model = await parser.Load("/desarrollo/mulesoft/raml-dotnet-tools/AMF.Tools.Tests/files/raml1/chinook-v1.raml");
            txtBox1.Text = model.WebApi.Name;
            
        }
    }
}
