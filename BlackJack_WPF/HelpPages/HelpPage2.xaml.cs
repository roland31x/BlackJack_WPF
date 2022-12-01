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
    /// Interaction logic for HelpPage2.xaml
    /// </summary>
    public partial class HelpPage2 : Page
    {
        public HelpPage2()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            //App.HowToPlayWindow.ParentFrameHelper.Navigate(.pag1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.HowToPlayWindow.ParentFrameHelper.Navigate(new HelpPage3());
        }

        private void ButtonB_Click(object sender, RoutedEventArgs e)
        {
            App.HowToPlayWindow.ParentFrameHelper.Navigate(new HelpPage1()) ;
        }
    }
}
