using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Media;
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



        public static BlackJack.Deck myDeck;
        public static BlackJack.BlackJackStats BlackJackGame;


        public static WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        public static bool MusicPlaying = true;

        
        public static void PlayMusic()
        {
            wplayer.URL = EnvPath.Sounds + "/music.mp3";
            wplayer.settings.volume = 30;
            wplayer.settings.autoStart = true;
            wplayer.settings.setMode("loop", true);
            MusicPlaying = true;
        }
        public static void StopMusic()
        {
            wplayer.controls.stop();
            MusicPlaying = false;
        }
    }
}
