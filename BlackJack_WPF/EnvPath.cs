using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace BlackJack_WPF
{
    public static class EnvPath
    {
        private readonly static string _appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private readonly static string _temp = Path.GetTempPath() + "/" + "blackjack-r31x-temp-release";
        readonly static string AppName = "Blackjack";
        readonly static string Publisher = "roland31x";
        public readonly static string PubFolder = _appData + "/" + Publisher;
        public readonly static string AppFolder = _appData + "/" + Publisher + "/" + AppName;
        public readonly static string HSDataPath = AppFolder + "/hs.data";
        public readonly static string UserDataPath = AppFolder + "/user.data";
        public readonly static string TempPath = _temp;
        public readonly static string Sounds = TempPath + "/" + "sounds";

        public static Task InitGameFolders()
        {
            if (!Directory.Exists(PubFolder))
            {
                Directory.CreateDirectory(PubFolder);
            }
            if (!Directory.Exists(AppFolder))
            {
                Directory.CreateDirectory(AppFolder);
            }
            if (!Directory.Exists(TempPath))
            {
                Directory.CreateDirectory(TempPath);
            }
            if (!Directory.Exists(Sounds))
            {
                Directory.CreateDirectory(Sounds);
            }
            LoadSounds();
            return Task.CompletedTask;
        }

        static void LoadSounds()
        {
            string baseResourcePath = "BlackJack_WPF.Sounds.";
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get all resource names in the assembly
            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (string path in resourceNames)
            {
                if (path.StartsWith(baseResourcePath))
                {
                    string soundfile = Sounds + "/" + path.Split(baseResourcePath).Last();
                    if (File.Exists(soundfile))
                    {
                        continue;
                    }
                    File.Create(soundfile).Dispose();
                    using (Stream stream = assembly.GetManifestResourceStream(path))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);
                        File.WriteAllBytes(soundfile, buffer);
                    }
                }
            }
        }
    }
}
