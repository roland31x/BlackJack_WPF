using Microsoft.VisualBasic;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace BlackJack_WPF
{   
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        BlackJack.BlackJackStats thisGame = App.BlackJackGame;

        BlackJack.Round thisRound = new BlackJack.Round(); 
        
        ImageSourceConverter imgConv = new ImageSourceConverter();
        
        List<Image> images = new List<Image>();
        
        List<Image> d_images = new List<Image>();
        
        ImageSource? DealersHiddenCard;

        bool Natural_P = false;
        bool Natural_D = false;
        
        int DealersHValue;

        public int cardcounter = 0;
        public int CardOffset = 80;
        public int d_cardcounter = 0;
        public GamePage()
        {
            InitializeComponent();
            Play();         
        }
        async void Play()
        {
            Can_DD();
            UpdateUI();
            ButtonCanvas.IsEnabled = false;
            await Task.Delay(500);
            await DrawNewCardTimed();
            await D_DrawNewCardTimed();
            if (thisRound.LastCardVal == 11 && thisGame.Balance > 0)
            {
                Insurance_Button.Visibility = Visibility.Visible;
            }
            await DrawNewCardTimed();
            await D_DrawNewCardTimed();           
            
            if (thisRound.NaturalBlackJackCheck_P())
            {
                Natural_P = true;
                DealersTurn();
                return;
            }
            ButtonCanvas.IsEnabled = true;
            
        }

        async void DoubleDown(object sender, RoutedEventArgs e)
        {
            Insurance_Button.Visibility = Visibility.Collapsed;
            thisGame.Balance -= thisGame.CurrentBet;
            thisGame.CurrentBet *= 2;
            ButtonCanvas.Visibility = Visibility.Collapsed;
            await DrawNewCardTimed();
            if (thisRound.HandVal >= 21)
            {
                if (thisRound.HandVal == 21)
                {
                    DealersTurn();
                    return;
                }
                if (thisRound.HandVal > 21)
                {
                    FinalCompare();
                    return;
                }
            }
            else
            {
                DealersTurn();
            }
        }
        void Can_DD()
        {
            if (thisGame.Balance - thisGame.CurrentBet < 0)
            {
                DD_Button.Visibility = Visibility.Collapsed;
                Canvas.SetLeft(ButtonCanvas, 169);
            }
        }

        void Stand(object sender, RoutedEventArgs e)
        {
            Insurance_Button.Visibility= Visibility.Collapsed;
            DealersTurn();
        }

        async void Hit(object sender, RoutedEventArgs e)
        {
            Insurance_Button.Visibility = Visibility.Collapsed;
            ButtonCanvas.IsEnabled = false;
            await DrawNewCardTimed();                      
            if(thisRound.HandVal >= 21)
            {
                if(thisRound.HandVal == 21)
                {                  
                    DealersTurn();
                    return;
                }
                if(thisRound.HandVal > 21)
                {
                    ButtonCanvas.Visibility = Visibility.Collapsed;
                    FinalCompare();
                    return;
                }
            }
            ButtonCanvas.IsEnabled = true;
        }

        private void Insurance_Button_Click(object sender, RoutedEventArgs e)
        {
            Insurance_Button.IsEnabled = false;
            Insurance_Button.Content = "Insured";
            thisRound.WasInsured = true;
            InsuranceWindow insurance = new InsuranceWindow();
            insurance.ShowDialog();
            BetVal.Text = "Current Bet: " + thisGame.CurrentBet +"$";
            InsVal.Visibility = Visibility.Visible;
            InsVal.Text = $"Insurance bet: " + App.BlackJackGame.InsuranceBet + "$";
            App.BlackJackGame.Balance -= App.BlackJackGame.InsuranceBet;

        }
        static void CardSound()
        {
            SoundPlayer sound = new SoundPlayer("Sounds/Card-flip-sound-effect.wav");
            sound.Play();
        }
        static void WinSound()
        {
            SoundPlayer sound = new SoundPlayer("Sounds/app-29.wav");
            sound.Play();
        }
        static void LoseSound()
        {
            SoundPlayer sound = new SoundPlayer("Sounds/Lose-sound.wav");
            sound.Play();
        }
        private void UpdateUI()
        {
            
            HandVal.Text = "Your Hand: " + thisRound.HandVal;
            if (thisRound.HandValSoft != 0 && thisRound.HandValSoft < thisRound.HandVal)
            {
                HandVal.Text = "Your Hand: " + thisRound.HandVal + " / " + thisRound.HandValSoft;
            }
            DealerVal.Text = "Dealer Hand: " + thisRound.D_HandVal;
            if (thisRound.D_HandValSoft != 0 && thisRound.D_HandValSoft < thisRound.D_HandVal)
            {
                DealerVal.Text = "Your Hand: " + thisRound.D_HandVal + " / " + thisRound.D_HandValSoft;
            }
            BetVal.Text = "Current Bet: " + thisGame.CurrentBet+"$";
            DealerVal_Copy.Text = $"{App.myDeck.CardsLeft()}";
        }
        private async void InsuranceCheck()
        {
            ButtonCanvas.Visibility = Visibility.Collapsed;
            if(thisRound.HandVal > 21)
            {
                thisRound.D_AddCard(DealersHValue);
                d_images[1].Source = DealersHiddenCard;
                thisRound.D_HandCalc();
            }
            UpdateUI();
            await Task.Delay(1000);
            if (thisRound.NaturalBlackJackCheck_D())
            {
                MessageBox.Show($"Insurance paid off! You get ${thisGame.InsuranceBet * 2} back.");
                thisGame.Balance += thisGame.InsuranceBet * 3;
            }
        }
        private async void DealersTurn()
        {
            CardSound();
            await Task.Delay(200);
            ButtonCanvas.Visibility= Visibility.Collapsed;
            thisRound.D_AddCard(DealersHValue);
            d_images[1].Source = DealersHiddenCard;
            thisRound.D_HandCalc();
            UpdateUI();
            await Task.Delay(700);
            if (thisRound.NaturalBlackJackCheck_D())
            {
                Natural_D = true;
                FinalCompare();
                return;
            }
            while (thisRound.D_HandVal < 17)
            {
                await D_DrawNewCardTimed();
            }
            FinalCompare();
        }
        private void FinalCompare()
        {
            if (thisRound.WasInsured) 
            {
                InsuranceCheck();
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
            if (thisRound.HandVal > 21) 
            {
                GameEndScreen(3);
                return;
            }
            if (thisRound.HandVal < thisRound.D_HandVal && thisRound.D_HandVal <= 21)
            {
                GameEndScreen(0);
                return;
            }
            if (thisRound.HandVal > thisRound.D_HandVal)
            {
                GameEndScreen(1);
                return;
            }
            if (thisRound.HandVal == thisRound.D_HandVal)
            {
                GameEndScreen(2);
                return;
            }
            if (thisRound.D_HandVal > 21)
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
                case 0: EndScreen.Text = $"You lost {thisGame.CurrentBet}$"; LoseSound();break;
                case 1: EndScreen.Text = $"You won {thisGame.CurrentBet}$";thisGame.Balance += thisGame.CurrentBet * 2; WinSound(); break;
                case 2: EndScreen.Text = $"PUSH!"; thisGame.Balance += thisGame.CurrentBet; break;
                case 3: EndScreen.Text = $"BUST! You lost {thisGame.CurrentBet}$"; LoseSound(); break;
                case 4: EndScreen.Text = $"DEALER BUST! You won {thisGame.CurrentBet}$"; thisGame.Balance += thisGame.CurrentBet * 2; WinSound(); break;
                case 10: EndScreen.Text = $"DEALER BLACKJACK! You lost {thisGame.CurrentBet}$"; LoseSound();
                    break;
                case 11: EndScreen.Text = $"BLACKJACK! You won {thisGame.CurrentBet}$"; thisGame.Balance += thisGame.CurrentBet * 2; WinSound();
                    break;
                case 12: EndScreen.Text = $"NATURAL BLACKJACKS!!! PUSH!"; thisGame.Balance += thisGame.CurrentBet; break;
            }
            NewGameButton.Visibility = Visibility.Visible;
            NewGameButton.Focus();
        }
        private void DrawNewCard()
        {
            cardcounter++;           
            var cardf = new Image()
            {
                Source = (ImageSource)imgConv.ConvertFromString(App.myDeck.DrawCard(thisRound)),
                Height = 160,
                Width = 110,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };
            thisRound.AddCard();
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
            
            thisRound.HandCalc();
            UpdateUI();

        }
        private void D_DrawNewCard()
        {
            d_cardcounter++;
            if(d_cardcounter == 2)
            {
                DealersHiddenCard = (ImageSource)imgConv.ConvertFromString(App.myDeck.DrawCard(thisRound));
                DealersHValue = thisRound.LastCardVal;
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
                UpdateUI();
                return;
            }
            var cardf = new Image()
            {
                Source = (ImageSource)imgConv.ConvertFromString(App.myDeck.DrawCard(thisRound)),
                Height = 160,
                Width = 110,
                Stretch = Stretch.Fill,
                Visibility = Visibility.Visible,
            };
            thisRound.D_AddCard();
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
            thisRound.D_HandCalc();
            UpdateUI();
        }
        async Task DrawNewCardTimed()
        {
            CardSound();
            await Task.Delay(200);          
            DrawNewCard();
            await Task.Delay(700);
        }
        async Task D_DrawNewCardTimed()
        {           
            CardSound();
            await Task.Delay(200);
            D_DrawNewCard();
            await Task.Delay(700);
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            thisGame.GamesPlayed++;
            if (App.myDeck.CardsLeft() < 22)
            {

                App.myDeck = BlackJack.Deck.NewDeck();
                MessageBox.Show("Deck is low on cards, shuffling a new deck...");
            }
            if(thisGame.Balance <= 0)
            {
                LoseSound();
                if(thisGame.GetScore() > HighScore.LoadSave().GetScore())
                {
                    HighScoresWindow wind = new HighScoresWindow();
                    wind.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"You ran out of money!{Environment.NewLine}You didn't beat the current highscore.{Environment.NewLine}You can try again starting with 500$!.");
                }
                App.BlackJackGame = new BlackJack.BlackJackStats();
                App.myDeck = BlackJack.Deck.NewDeck();
            }
            App.BlackJackGame.CheckScore();

            App.ParentWindowRef.ParentFrame.Navigate(new BettingPage());
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(NewGameButton.Visibility == Visibility.Visible)
                {
                    NewGame(sender, e);
                }              
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
