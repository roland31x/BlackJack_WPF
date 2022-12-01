using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BlackJack_WPF
{
    public class BlackJack
    {
        public static int Balance = 500;
        public static int CurrentBet { get; set; }
        public static int InsuranceBet { get; set; }
        public class Game
        {
            public bool WasInsured { get; set; }
            public List<int> CardValues { get; set; }
            public List<int> D_CardValues { get; set; }
            public int LastCardVal { get; set; }
            public int HandVal { get; set; }
            public int D_HandVal { get; set; }
            public Game()
            {
                CardValues = new List<int>();
                D_CardValues = new List<int>();
                WasInsured = false;
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
            public void HandCalc()
            {
                HandVal = 0;
                foreach (int i in CardValues)
                {
                    HandVal += i;
                }
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
            public void D_HandCalc()
            {
                D_HandVal = 0;
                foreach (int i in D_CardValues)
                {
                    D_HandVal += i;
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
            public string DrawCard(Game game)
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
                    case "11":cardName = "jack";break;
                    case "12":cardName = "queen";break;
                    case "13":cardName = "king";break;
                    case "14":cardName = "ace";break;
                    default: break;
                }
                switch (cardType)
                {
                    case "1": cardType = "clubs"; break;
                    case "2": cardType = "diamonds"; break;
                    case "3": cardType = "hearts"; break;
                    case "4": cardType = "spades"; break;
                }
                card.Append("CardPics/");
                card.Append(cardName);
                card.Append("_of_");
                card.Append(cardType);
                card.Append(".png");
                return card.ToString();
            }
            public string ShowCard()
            {
                return this.CurrentDeck.Pop().ToString();
            }
            public static Deck NewDeck()
            {
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
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }       
    }
}
