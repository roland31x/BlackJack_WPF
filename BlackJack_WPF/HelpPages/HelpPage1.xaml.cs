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

namespace BlackJack_WPF.HelpPages
{
    /// <summary>
    /// Interaction logic for HelpPage1.xaml
    /// </summary>
    public partial class HelpPage1 : Page
    {
        public HelpPage1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.HowToPlayWindow.ParentFrameHelper.Navigate(new HelpPage2());
        }
    }
}
