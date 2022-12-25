using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_WPF
{
    public class Profile
    {
        public static void SaveName(BlackJack.BlackJackStats s)
        {
            string path = "Profile.data";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                AddText(fs, "Username: " + s.GetName());
            }
        }
        public static string CreateProfile()
        {
            string path = "Profile.data";
            if(File.Exists(path))
            {
                return LoadProfile(path);
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    AddText(fs, "Username: Player");
                    
                }
                return "Player";
            }
        }
        private static string CreateProfileNew()
        {
            string path = "Profile.data";
            File.Delete(path);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                AddText(fs, "Username: Player");
                
            }
            return "Player";
        }

        private static string LoadProfile(string path)
        {
            try
            {
                StringBuilder score = new StringBuilder();
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] b = new byte[1];
                    //UTF8Encoding tmp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        score.Append((char)b[0]);
                    }
                }
                string name = score.ToString().Split(':')[1].Replace(" ", "");
                return name;
            }
            catch (Exception)
            {
                return CreateProfileNew();
            }           
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
