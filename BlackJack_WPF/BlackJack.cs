using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlackJack_WPF
{
    public class BlackJack
    {      
        public class BlackJackStats
        {
            public int Balance { get; set; }
            public int GamesPlayed { get; set; }
            public int CurrentBet { get; set; }
            public int InsuranceBet { get; set; }
            protected string Name { get; set; }         
            protected int MaxBal { get; set; }
            public BlackJackStats()
            {
                Balance = 500;
                MaxBal = 500;
                GamesPlayed = 0;
                Name = Profile.GetName();
            }
            public void SetName(string s)
            {
                this.Name = s;
            }
            public string GetName()
            {
                return this.Name;
            }
            public int GetScore()
            {
                return this.MaxBal;
            }
            public void CheckScore()
            {
                if(this.Balance > this.MaxBal)
                {
                    this.MaxBal = this.Balance;
                }
            }
        }
        public class Round
        {
            public bool WasInsured { get; set; }
            public List<int> CardValues { get; set; }
            public List<int> D_CardValues { get; set; }
            public int LastCardVal { get; set; }
            public int HandVal { get; set; }
            public int D_HandVal { get; set; }
            public int HandValSoft { get; set; }
            public int D_HandValSoft { get; set; }
            public Round()
            {
                CardValues = new List<int>();
                D_CardValues = new List<int>();
                WasInsured = false;
                D_HandValSoft = 0;
                HandValSoft = 0;
            }
            public void AddCard()
            {
                CardValues.Add(LastCardVal);
            }
            public void D_AddCard()
            {
                D_CardValues.Add(LastCardVal);
            }
            public void D_AddCard(int f)
            {
                D_CardValues.Add(f);
            }
           
            public bool NaturalBlackJackCheck_P()
            {
                if (HandVal == 21 && CardValues.Count == 2)
                {
                    return true;
                }
                else return false;
                
            }
            public bool NaturalBlackJackCheck_D()
            {
                if (D_HandVal == 21 && D_CardValues.Count == 2)
                {
                    return true;
                }
                else return false;
            } 
            public void HandCalc()
            {
                HandVal = 0;
                foreach (int i in CardValues)
                {
                    HandVal += i;
                }
                if (CardValues.Contains(11))
                {
                    HandValSoft = HandVal - 10;
                    if (HandVal > 21)
                    {
                        CardValues.Remove(11);
                        CardValues.Add(1);
                        HandVal = HandValSoft;
                        HandValSoft = 0;
                    }
                }
            }
            public void D_HandCalc()
            {
                D_HandVal = 0;
                foreach (int i in D_CardValues)
                {
                    D_HandVal += i;
                }
                if (D_CardValues.Contains(11))
                {
                    D_HandValSoft = D_HandVal - 10;
                    if(D_HandVal > 21)
                    {
                        D_CardValues.Remove(11);
                        D_CardValues.Add(1);
                        D_HandVal = D_HandValSoft;
                        D_HandValSoft = 0;
                    }
                }
            }
        }
        public class Deck
        {
            Stack<Vector2> CurrentDeck = new Stack<Vector2>();
            public Deck()
            {               
                List<Vector2> newDeck = new List<Vector2>();
                for (int x = 1; x <= 4; x++)
                {
                    for (int y = 2; y <= 14; y++)
                    {
                        newDeck.Add(new Vector2(x, y));
                    }
                }
                Shuffle(newDeck);
                foreach(Vector2 v in newDeck)
                {
                    CurrentDeck.Push(v);
                }
            }
            public BitmapImage CardImage(Round game)
            {
                StringBuilder card = new StringBuilder();
                Vector2 v;
                v = this.CurrentDeck.Pop();
                int value = Convert.ToInt16(v.Y);
                if (value > 10 && value < 14)
                {
                    value = 10;
                }
                else if (value == 14)
                {
                    value = 11;
                }
                game.LastCardVal= value;
                string cardName = v.Y.ToString();
                string cardType = v.X.ToString();
                switch (cardName) 
                {
                    case "11":cardName = "jack"; break;
                    case "12":cardName = "queen"; break;
                    case "13":cardName = "king"; break;
                    case "14":cardName = "ace"; break;
                    default: break;
                }
                switch (cardType)
                {
                    case "1": cardType = "clubs"; break;
                    case "2": cardType = "diamonds"; break;
                    case "3": cardType = "hearts"; break;
                    case "4": cardType = "spades"; break;
                }
                card.Append("BlackJack_WPF.CardPics.");
                card.Append(cardName);
                card.Append("_of_");
                card.Append(cardType);
                card.Append(".png");

                BitmapImage tor = new BitmapImage();
                tor.BeginInit();
                try
                {
                    using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(card.ToString()))
                    {
                        tor.StreamSource = s;
                    }
                    tor.EndInit();
                    return tor;
                }
                catch
                {
                    tor.EndInit();
                    return new BitmapImage();
                }
            }
            public int CardsLeft()
            {
                return this.CurrentDeck.Count;
            }
            public void ShuffleDeck()
            {
                ShuffleSound();
                List<Vector2> newdeck = new List<Vector2>();
                while (this.CurrentDeck.Count > 0)
                {
                    newdeck.Add(this.CurrentDeck.Pop());
                }
                Shuffle(newdeck);
                foreach (Vector2 v in newdeck)
                {
                    CurrentDeck.Push(v);
                }
                return;
            }
            public static Deck NewDeck()
            {
                ShuffleSound();
                return new Deck();
            }
            private static Random rng = new Random();
            private static void Shuffle<T>(IList<T> list)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    (list[n], list[k]) = (list[k], list[n]);
                }
            }
            static void ShuffleSound()
            {
                SoundPlayer sound = new SoundPlayer(EnvPath.Sounds + "/card-shuffle.wav");
                sound.Play();
            }
        }       
    }
}
