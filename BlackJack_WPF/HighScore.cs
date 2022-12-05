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
        protected string Name { get; set; }
        protected int Score { get; set; }
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
        public int GetScore()
        {
            return this.Score;
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
        public void Save()
        {
            string path = "EnSave.data";
            string temp = "Temp.data";
            File.Delete(path);
            using (FileStream fs = new FileStream(temp, FileMode.OpenOrCreate))
            {
                AddText(fs, this.ToString());
            }
            EncryptFile(temp, path);
        }
        public static HighScore LoadSave()
        {
            string path = "EnSave.Data";
            string temp = "Temp.data";
            StringBuilder score = new StringBuilder();
            try
            {
                DecryptFile(path, temp);
            }
            catch(Exception)
            {
                MessageBox.Show("Save data corrupt, consider deleting the .data files in the .exe directory.");
                return new HighScore("ERROR", 404);
            }
            using (FileStream fs = File.OpenRead(temp))
            {
                byte[] b = new byte[1];
                //UTF8Encoding tmp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    score.Append((char)b[0]);
                }
            }
            score.Replace(" ", "");
            score.Replace("$", "");
            File.Delete(temp);
            try
            {
                HighScore loaded = new HighScore(score.ToString().Split(':')[0], Convert.ToInt32(score.ToString().Split(':')[1]));
                return loaded;
            }
            catch (Exception)
            {
                MessageBox.Show("Save data corrupt, consider deleting the .data files in the .exe directory.");
                return new HighScore("ERROR", 404);
            }
            
        }
        public static void CreateDefaultSave()
        {
            string path = "Save.data";
            string path2 = "EnSave.data";
            if (File.Exists(path2))
            {
                return;
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    AddText(fs, $"DEFAULT JACK: 3000$");
                }
                EncryptFile(path, path2);
                File.Delete(path);
            }
        }
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        private static void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"R0L2004X"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {
                MessageBox.Show("Encryption failed!", "Error");
            }
}
        ///<summary>
        /// Steve Lydford - 12/05/2008.
        ///
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        private static void DecryptFile(string inputFile, string outputFile)
        {

            {
                string password = @"R0L2004X"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
                
            }
        }
    }
}
