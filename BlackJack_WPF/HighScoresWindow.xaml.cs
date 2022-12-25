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
    /// Interaction logic for HighScoresWindow.xaml
    /// </summary>
    public partial class HighScoresWindow : Window
    {
        public HighScoresWindow()
        {
            InitializeComponent();
            PlayerName_box.Text = App.BlackJackGame.GetName();
            HighScoreBox.Text = HighScore.LoadSave().ToString();
            BalBox.Text = "HIGHEST BALANCE: " + App.BlackJackGame.GetScore().ToString() +"$";
            if (App.BlackJackGame.GetScore() < HighScore.LoadSave().GetScore())
            {
                SubmitB.IsEnabled = false;
            }
            else SubmitB.IsEnabled = true;
        }

        private void SubmitB_Click(object sender, RoutedEventArgs e)
        {
            HighScore tmp = new HighScore(App.BlackJackGame);
            tmp.Save();
            this.Close();
        }
    }
}
