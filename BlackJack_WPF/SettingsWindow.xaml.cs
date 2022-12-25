using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            TextBName.Text = App.BlackJackGame.GetName();
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
            else if(TextBName.Text == App.BlackJackGame.GetName())
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

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NameSetButton.Focus();
                if (NameSetButton.IsEnabled)
                {
                    SetNameB(sender, e);
                }
                NameSetButton.Focus();
            }
            if (TextBName.IsFocused)
            {
                return;
            }
            if(e.Key == Key.M)
            {
                if (MusicButton.IsChecked == false)
                {
                    MusicButton.IsChecked = true;
                    ToggleButton_Checked(sender, e);
                }
                else
                {
                    MusicButton.IsChecked = false;
                    ToggleButton_Unchecked(sender, e);
                }
            }
            if(e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void QuitGameButton_Click(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef.Close();
            this.Close();
        }
    }
}
