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

namespace BlackJack_WPF
{
    /// <summary>
    /// Interaction logic for BettingPage.xaml
    /// </summary>
    public partial class BettingPage : Page
    {
        static int betval;
        public BettingPage()
        {
            InitializeComponent();
            UpdateUI();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(BetInput.Text, out betval))
            {              
                StartGame.IsEnabled = false;
                StartGame.Opacity = 0.1;
                return;
            }
            if (BlackJack.Balance - betval < 0)
            {
                StartGame.IsEnabled = false;
                StartGame.Opacity = 0.1;
                return;
            }
            else
            {

                StartGame.IsEnabled = true;
                StartGame.Opacity = 100;
            }
        }
        void UpdateUI()
        {
            CurrentBal.Text = $"Available Balance:{Environment.NewLine}{BlackJack.Balance}$";
        }       

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            BlackJack.Balance -= betval;
            BlackJack.CurrentBet = betval;
            App.ParentWindowRef.ParentFrame.Navigate(new GamePage());           
        }
    }
}
