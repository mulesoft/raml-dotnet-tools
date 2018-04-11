using AMF.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
