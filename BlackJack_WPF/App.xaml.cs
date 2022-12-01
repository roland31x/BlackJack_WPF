using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BlackJack_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow ParentWindowRef;
        public static HelpPages.Help HowToPlayWindow;
        
        public static BlackJack.Deck myDeck = new BlackJack.Deck();
        public static BlackJack.BlackJackStats BlackJackGame = new BlackJack.BlackJackStats();
    }
}
