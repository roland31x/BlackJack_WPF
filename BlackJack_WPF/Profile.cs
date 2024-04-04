using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_WPF
{
    public static class Profile
    {
        private static string? name = null;
        public static void SetName(string s)
        {
            File.WriteAllText(EnvPath.UserDataPath, s);
            name = s;
        }
        public static string GetName()
        {
            if(name == null)
            {
                try
                {
                    name = File.ReadAllText(EnvPath.UserDataPath);
                    return name;
                }
                catch (Exception e)
                {
                    SetName("Player");
                    return "Player";
                }
            }
            return name;
        }
    }
}
