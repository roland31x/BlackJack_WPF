using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
            DeckCount.Text = $"{App.myDeck.CardsLeft()}";
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
            if (App.BlackJackGame.Balance - betval < 0)
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
            CurrentBal.Text = $"Available Balance:{Environment.NewLine}{App.BlackJackGame.Balance}$";
        }
        void PlaySound()
        {
            SoundPlayer sound = new SoundPlayer("Sounds/coins-sound-fx.wav");
            sound.Play();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            PlaySound();
            App.BlackJackGame.Balance -= betval;
            App.BlackJackGame.CurrentBet = betval;
            App.ParentWindowRef.ParentFrame.Navigate(new GamePage());
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            App.myDeck.ShuffleDeck();
            Shuffle_b.IsEnabled = false;
            Shuffle_b.Content = "Shuffled";

        }

        private void NewDeck_Click(object sender, RoutedEventArgs e)
        {
            App.myDeck = BlackJack.Deck.NewDeck();
            NewDeck_b.IsEnabled = false;
            NewDeck_b.Content = "Done!";
            DeckCount.Text = $"{App.myDeck.CardsLeft()}";
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow set = new SettingsWindow();
            set.ShowDialog();
        }
    }
}
