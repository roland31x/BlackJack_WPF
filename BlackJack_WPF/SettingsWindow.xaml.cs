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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            TextBName.Text = Profile.CreateProfile();
            if (App.MusicPlaying)
            {
                MusicButton.IsChecked = false;
            }
            else MusicButton.IsChecked = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextBName.Text.Length > 10) 
            {
                NameSetButton.IsEnabled = false;
                return;
            }
            else if (TextBName.Text == "")
            {
                NameSetButton.IsEnabled = false;
                return;
            }
            else if(TextBName.Text.Contains(':'))
            {
                NameSetButton.IsEnabled = false;
                return;
            }
            else if(TextBName.Text == Profile.CreateProfile())
            {
                NameSetButton.IsEnabled = false;
                return;
            }
            else
            NameSetButton.IsEnabled = true;
            
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            MusicButton.Content = "OFF";
            App.StopMusic();
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            MusicButton.Content = "ON";
            App.PlayMusic();

        }

        private void SetNameB(object sender, RoutedEventArgs e)
        {
            App.BlackJackGame.SetName(TextBName.Text);
            Profile.SaveName(App.BlackJackGame);
            NameSetButton.IsEnabled = false;
        }
    }
}
