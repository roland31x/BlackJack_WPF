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

namespace BlackJack_WPF
{
    /// <summary>
    /// Interaction logic for InsuranceWindow.xaml
    /// </summary>
    public partial class InsuranceWindow : Window
    {
        int insbet;
        public InsuranceWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if(!int.TryParse(ins.Text,out insbet))
            {
                ins.IsEnabled = false;
            }
            if (insbet > BlackJack.CurrentBet * 2)
            {
                ins.IsEnabled = false;
            }
            else ins.IsEnabled = true;
        }
        private void ins_b_Click(object sender, RoutedEventArgs e)
        {
            BlackJack.InsuranceBet = insbet;
            this.Close();
        }
    }
}
