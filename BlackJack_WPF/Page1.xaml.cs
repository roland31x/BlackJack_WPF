using BlackJack_WPF.HelpPages;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    { 
        
        public Page1()
        {
            InitializeComponent();
            HighScore.CreateDefaultSave();
            UserNameBlock.Text = "Welcome, " + Profile.CreateProfile() + "!";
            this.Focusable = true;
            Loaded += Page1_Loaded;
            
        }

        private void Page1_Loaded(object sender, RoutedEventArgs e)
        {
            startb.Focus();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef.ParentFrame.Navigate(new BettingPage());
        }

        private void HowToPlay_Click(object sender, RoutedEventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void HighscoresB_Click(object sender, RoutedEventArgs e)
        {           
            HighScoresWindow hs = new HighScoresWindow();
            hs.ShowDialog();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow set = new SettingsWindow();
            set.ShowDialog();
            UserNameBlock.Text = "Welcome, " + Profile.CreateProfile() + "!";
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Play_Button_Click(sender, e);
            }
            
        }
    }
}
