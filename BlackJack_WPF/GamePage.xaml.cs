using Microsoft.VisualBasic;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace BlackJack_WPF
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        BlackJack.Deck myDeck = new BlackJack.Deck();
        BlackJack.Game nGame = new BlackJack.Game(); 
        
        ImageSourceConverter imgConv = new ImageSourceConverter();
        
        List<Image> images = new List<Image>();
        
        List<Image> d_images = new List<Image>();
        
        ImageSource DealersHiddenCard;

        bool Natural_P = false;
        bool Natural_D = false;
        
        int DealersHValue;
        public int cardcounter = 0;
        public int CardOffset = 80;
        public int d_cardcounter = 0;
        public GamePage()
        {
            InitializeComponent();

            DrawNewCard();
            DrawNewCard();
            
            D_DrawNewCard();
            if (nGame.LastCardVal == 11)
            {
                Insurance_Button.Visibility = Visibility.Visible;
            }
            D_DrawNewCard();
            if (nGame.NaturalBlackJackCheck_P())
            {
                Natural_P = true;
                DealersTurn();
                FinalCompare();
            }
            DealersHiddenCard = (ImageSource)imgConv.ConvertFromString(myDeck.DrawCard(nGame));
            DealersHValue = nGame.LastCardVal;
        }


        private void DoubleDown(object sender, RoutedEventArgs e)
        {
            Insurance_Button.IsEnabled = false;
            DD_Button.IsEnabled = false;
            Stand_Button.IsEnabled = false;
            Hit_Button.IsEnabled = false;
            BlackJack.Balance -= BlackJack.CurrentBet;
            BlackJack.CurrentBet *= 2;           
            Hit(sender, e);           
            DealersTurn();
        }

        private void Stand(object sender, RoutedEventArgs e)
        {
            Insurance_Button.IsEnabled = false;

            DealersTurn();
        }

        private void Hit(object sender, RoutedEventArgs e)
        {
            Insurance_Button.IsEnabled = false;
            DrawNewCard();                      
            if(nGame.HandVal >= 21)
            {
                if(nGame.HandVal == 21)
                {                  
                    DealersTurn();
                }
                if(nGame.HandVal > 21)
                {
                    ButtonCanvas.Visibility = Visibility.Collapsed;
                    GameEndScreen(3);
                }
            }
        }

        private void Insurance_Button_Click(object sender, RoutedEventArgs e)
        {
            Insurance_Button.IsEnabled = false;
            Insurance_Button.Content = "Insured";
            nGame.WasInsured = true;
            InsuranceWindow insurance = new InsuranceWindow();
            insurance.ShowDialog();
            BetVal.Text = "Current Bet: " + BlackJack.CurrentBet +"$";
            InsVal.Visibility = Visibility.Visible;
            InsVal.Text = $"Insurance bet: " + BlackJack.InsuranceBet + "$";

        }
        private void UpdateUI()
        {
            HandVal.Text = "Your Hand: " + nGame.HandVal;
            DealerVal.Text = "Dealer Hand: " + nGame.D_HandVal;
            BetVal.Text = "Current Bet: " + BlackJack.CurrentBet;
        }
        private void DealersTurn()
        {
            ButtonCanvas.Visibility= Visibility.Collapsed;
            nGame.D_AddCard(DealersHValue);
            d_images[1].Source = DealersHiddenCard;
            nGame.D_HandCalc();
            UpdateUI();
            if (nGame.NaturalBlackJackCheck_D())
            {
                Natural_D = true;
                FinalCompare();
                return;
            }
            while (nGame.D_HandVal < 17)
            {
                D_DrawNewCard();
            }
            FinalCompare();
        }
        private void FinalCompare()
        {
            if (nGame.WasInsured) 
            {
                if(Natural_D)
                {
                    MessageBox.Show($"Insurance paid off! You get ${BlackJack.InsuranceBet * 2} back.");
                    BlackJack.Balance += BlackJack.InsuranceBet * 2;
                }
            }
            if (Natural_D && !Natural_P)
            {
                GameEndScreen(10);
                return;
            }
            if (Natural_P && Natural_D)
            {
                GameEndScreen(12);
                return;
            }
            if (Natural_P && !Natural_D)
            {
                GameEndScreen(11);
                return;
            }
            if (nGame.HandVal > 21) 
            {
                GameEndScreen(3);
                return;
            }
            if (nGame.HandVal < nGame.D_HandVal && nGame.D_HandVal <= 21)
            {
                GameEndScreen(0);
                return;
            }
            if (nGame.HandVal > nGame.D_HandVal)
            {
                GameEndScreen(1);
                return;
            }
            if (nGame.HandVal == nGame.D_HandVal)
            {
                GameEndScreen(2);
                return;
            }
            if (nGame.D_HandVal > 21)
            {
                GameEndScreen(4);
                return;
            }
        }
        private void GameEndScreen(int cases)
        {
            EndScreen.Visibility = Visibility.Visible;
            switch (cases)
            {
                case 0:EndScreen.Text = $"You lost {BlackJack.CurrentBet}$";break;
                case 1:EndScreen.Text = $"You won {BlackJack.CurrentBet}$";BlackJack.Balance += BlackJack.CurrentBet * 2; break;
                case 2:EndScreen.Text = $"PUSH!"; BlackJack.Balance += BlackJack.CurrentBet; break;
                case 3: EndScreen.Text = $"BUST! You lost {BlackJack.CurrentBet}$"; break;
                case 4: EndScreen.Text = $"DEALER BUST! You won {BlackJack.CurrentBet}$"; BlackJack.Balance += BlackJack.CurrentBet * 2; break;
                case 10: EndScreen.Text = $"DEALER BLACKJACK! You lost {BlackJack.CurrentBet}$"; break;
                case 11: EndScreen.Text = $"BLACKJACK! You won {BlackJack.CurrentBet}$"; BlackJack.Balance += BlackJack.CurrentBet * 2; break;
                case 12: EndScreen.Text = $"NATURAL BLACKJACKS!!! PUSH!"; BlackJack.Balance += BlackJack.CurrentBet; break;
            }
            NewGameButton.Visibility = Visibility.Visible;
        }
        private void DrawNewCard()
        {
            cardcounter++;           
            var cardf = new Image()
            {
                Source = (ImageSource)imgConv.ConvertFromString(myDeck.DrawCard(nGame)),
                Height = 160,
                Width = 110,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };
            nGame.AddCard();
            MainCanvas.Children.Add(cardf);
            images.Add(cardf);
            Canvas.SetTop(cardf, 437);
            Canvas.SetLeft(cardf, 650);
            foreach (Image s in images)
            {
                Canvas.SetLeft(s, Canvas.GetLeft(s) - CardOffset);
            }
            if (cardcounter == 7)
            {
                CardOffset = 40;
                for (int i = 0; i < images.Count; i++ )
                {
                    Canvas.SetLeft(images[i], Canvas.GetLeft(images[i]) + (40 * (images.Count - i)));
                }
            }
            nGame.HandCalc();
            UpdateUI();

        }
        private void D_DrawNewCard()
        {
            d_cardcounter++;
            if(d_cardcounter == 2)
            {
                DealersHiddenCard = (ImageSource)imgConv.ConvertFromString(myDeck.DrawCard(nGame));
                DealersHValue = nGame.LastCardVal;
                var cardh = new Image()
                {
                    Source = (ImageSource)imgConv.ConvertFromString("CardPics/card_back.png"),
                    Height = 160,
                    Width = 110,
                    Stretch = Stretch.Fill,
                    Visibility = Visibility.Visible,
                };
                MainCanvas.Children.Add(cardh);
                d_images.Add(cardh);
                Canvas.SetTop(cardh, 79);
                Canvas.SetLeft(cardh, 650);
                foreach (Image s in d_images)
                {
                    Canvas.SetLeft(s, Canvas.GetLeft(s) - CardOffset);
                }
                return;
            }
            var cardf = new Image()
            {
                Source = (ImageSource)imgConv.ConvertFromString(myDeck.DrawCard(nGame)),
                Height = 160,
                Width = 110,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };
            nGame.D_AddCard();
            MainCanvas.Children.Add(cardf);
            d_images.Add(cardf);
            Canvas.SetTop(cardf, 79);
            Canvas.SetLeft(cardf, 650);
            foreach (Image s in d_images)
            {
                Canvas.SetLeft(s, Canvas.GetLeft(s) - CardOffset);
            }
            if (cardcounter == 7)
            {
                CardOffset = 40;
                for (int i = 0; i < d_images.Count; i++)
                {
                    Canvas.SetLeft(images[i], Canvas.GetLeft(images[i]) + (40 * (images.Count - i)));
                }
            }
            nGame.D_HandCalc();
            UpdateUI();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef.ParentFrame.Navigate(new BettingPage());
        }
    }
}
