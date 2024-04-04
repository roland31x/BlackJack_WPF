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

                byte[] todecrypt = new byte[fs.Length];
                fs.Read(todecrypt, 0, (int)fs.Length);

                fs.Dispose();
                string decrypted = Decrypt(todecrypt, "R0L4NDAESENCRYPT");
                string name = decrypted.Split(':')[0];
                int score = int.Parse(decrypted.Split(':')[1].Split('$')[0].Trim());
                _loaded = new HighScore(name, score);
                return _loaded;
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading highscore file, reverting...");
                return OverrideDefaultSave();
            }
        }
        private void Reload()
        {
            FileStream fs = new FileStream(EnvPath.HSDataPath, FileMode.Open);

            byte[] todecrypt = new byte[fs.Length];
            fs.Read(todecrypt, 0, (int)fs.Length);

            fs.Dispose();
            string decrypted = Decrypt(todecrypt, "R0L4NDAESENCRYPT");
            string name = decrypted.Split(':')[0];
            int score = int.Parse(decrypted.Split(':')[1].Split('$')[0].Trim());
            _loaded = new HighScore(name, score);
        }
        public void SaveHS()
        {
            File.WriteAllBytes(EnvPath.HSDataPath, Encrypt(this.ToString(), "R0L4NDAESENCRYPT"));
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

        static byte[] Encrypt(string plainText, string key)
        {
            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {

                byte[] Key = Encoding.UTF8.GetBytes(key);
                byte[] IV = new byte[aes.BlockSize / 8];
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);

                using (MemoryStream ms = new MemoryStream())
                {

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return encrypted;
        }
        static string Decrypt(byte[] cipherText, string key)
        {
            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                byte[] Key = Encoding.UTF8.GetBytes(key);
                byte[] iv = new byte[aes.BlockSize / 8];
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, iv);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
