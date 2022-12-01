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
using System.Windows.Shapes;

namespace BlackJack_WPF.HelpPages
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : Window
    {
       public Help()
        {
            InitializeComponent();
        }
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.HowToPlayWindow = this;
            this.ParentFrameHelper.Navigate(new HelpPage1());
        }
    }
}
