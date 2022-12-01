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
            maxbet.Text = $"Max bet: {Math.Min((App.BlackJackGame.CurrentBet / 2), App.BlackJackGame.Balance)}$";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!int.TryParse(ins.Text, out insbet))
            {
                ins_b.IsEnabled = false;
            }
            if (insbet > Math.Min((App.BlackJackGame.CurrentBet / 2), App.BlackJackGame.Balance))
            {
                ins_b.IsEnabled = false;
            }
            else ins_b.IsEnabled = true;
        }
        private void ins_b_Click(object sender, RoutedEventArgs e)
        {
            App.BlackJackGame.InsuranceBet = insbet;
            this.Close();
        }
    }
}
