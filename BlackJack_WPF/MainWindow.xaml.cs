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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.ParentWindowRef = this;
            await App.Current.Dispatcher.InvokeAsync(() => EnvPath.InitGameFolders());

            HighScore.CreateDefaultSave();
            this.PreviewKeyDown += MusicCheck;
            this.PreviewKeyDown += SettingsMenu;

            App.PlayMusic();
            App.BlackJackGame = new BlackJack.BlackJackStats();
            App.myDeck = new BlackJack.Deck();

            this.ParentFrame.Navigate(new Page1());
        }
        private void MusicCheck(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.M)
            {
                if (App.MusicPlaying)
                {
                    App.StopMusic();
                }
                else 
                { 
                    App.PlayMusic(); 
                }
            }
        }
        private void SettingsMenu(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                object content = this.Content;
                SettingsWindow set = new SettingsWindow();
                set.ShowDialog();
                try
                {
                    if (((content as DockPanel).Children[0] as Frame).Content is Page1)
                    {
                        (((content as DockPanel).Children[0] as Frame).Content as Page1).UserNameBlock.Text = "Welcome, " + App.BlackJackGame.GetName() + "!";
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Something went wrong... send help.");
                }
            }
        }
        void ParentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ParentFrame.NavigationService.RemoveBackEntry();
        }
    }
}
