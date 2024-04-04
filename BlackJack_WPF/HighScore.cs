using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlackJack_WPF
{
    public class HighScore
    {
        private readonly string _key = "R0L4NDAESENCRYPT";
        protected string Name { get; set; }
        protected int Score { get; set; }
        private static HighScore? _loaded = null;
        public HighScore(BlackJack.BlackJackStats s)
        {
            Name = s.GetName();
            Score = s.GetScore();
        }
        private HighScore(string s, int n)
        {
            Name = s;
            Score = n;
        }
        public static HighScore OverrideDefaultSave()
        {
            HighScore def = new HighScore("Wild Bill", 3500);
            def.SaveHS();
            return def;
        }
        public static HighScore CreateDefaultSave()
        {
            if(!File.Exists(EnvPath.HSDataPath))
            {
                File.Create(EnvPath.HSDataPath).Dispose();
                return OverrideDefaultSave();
            }
            else
            {
                return LoadHS();
            } 
        }
        public static HighScore LoadHS()
        {
            if(_loaded != null)
            {
                return _loaded;
            }
            try
            {
                FileStream fs = new FileStream(EnvPath.HSDataPath, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string toencrypt = sr.ReadToEnd();
                sr.Close();
                fs.Dispose();
                string decrypted = DecryptString(toencrypt, "R0L4NDAESENCRYPT");
                string name = decrypted.Split(':')[0];
                int score = int.Parse(decrypted.Split(':')[1].Split('$')[0].Trim());
                _loaded = new HighScore(name, score);
                return _loaded;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return OverrideDefaultSave();
            }
        }
        private void Reload()
        {
            FileStream fs = new FileStream(EnvPath.HSDataPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string todecrypt = sr.ReadToEnd();
            sr.Close();
            fs.Dispose();
            string decrypted = DecryptString(todecrypt, "R0L4NDAESENCRYPTAAAAAAAAAAAAAAAAAAAAAAAAA");
            string name = decrypted.Split(':')[0];
            int score = int.Parse(decrypted.Split(':')[1].Split('$')[0].Trim());
            _loaded = new HighScore(name, score);
        }
        public void SaveHS()
        {
            File.WriteAllText(EnvPath.HSDataPath, EncryptString(this.ToString(), "R0L4NDAESENCRYPTAAAAAAAAAAAAAAAAAAAAAAAAA"));
            Reload();
        }
        public int GetScore()
        {
            return this.Score;
        }
        public string GetName()
        {
            return this.Name;
        }
        public void SetName(string s)
        {
            this.Name = s;
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(Name);
            str.Append(':');
            str.Append(' ');
            str.Append(Score);
            str.Append('$');
            return str.ToString();
        }

        static string EncryptString(string plaintext, string password)
        {
            return plaintext; // rework needed

        }

        static string DecryptString(string encrypted, string password)
        {
            return encrypted; // rework needed
        }

    }
}
