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
                    AddText(fs, $"DEFAULT-JACK: 3000$");
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

        private static readonly string _aesKeyString = "R0L4NDAESENCRYPT"; // Replace the ellipsis with your AES key

        public static void EncryptFile(string inputFilePath, string outputFilePath)
        {
            // Create a new AES algorithm with the key
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_aesKeyString);
                aes.GenerateIV();

                // Open the input file for reading
                using (var inputStream = File.OpenRead(inputFilePath))
                {
                    // Open the output file for writing
                    using (var outputStream = File.Create(outputFilePath))
                    {
                        // Write the IV to the output file
                        outputStream.Write(aes.IV, 0, aes.IV.Length);

                        // Create an encryptor to perform the encryption
                        using (var encryptor = aes.CreateEncryptor())
                        {
                            // Create a CryptoStream to encrypt the data
                            using (var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                            {
                                // Copy the data from the input stream to the CryptoStream
                                inputStream.CopyTo(cryptoStream);
                            }
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string inputFilePath, string outputFilePath)
        {
            // Create a new AES algorithm with the key
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_aesKeyString);

                // Open the input file for reading
                using (var inputStream = File.OpenRead(inputFilePath))
                {
                    // Read the IV from the input file
                    var iv = new byte[aes.IV.Length];
                    inputStream.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    // Open the output file for writing
                    using (var outputStream = File.Create(outputFilePath))
                    {
                        // Create a decryptor to perform the decryption
                        using (var decryptor = aes.CreateDecryptor())
                        {
                            // Create a CryptoStream to decrypt the data
                            using (var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                            {
                                // Copy the data from the CryptoStream to the output stream
                                cryptoStream.CopyTo(outputStream);
                            }
                        }
                    }
                }
            }
        }
    }
}
