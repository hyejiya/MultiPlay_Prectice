using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP.Authentication
{
    public class Login_Information
    {
        public static bool isLoggedIn => string.IsNullOrEmpty(s_id) == false;      
        public static string userKey { get; private set; }
        public static string nickname { get; set; } 

        private static string s_id;

        public static void Refresh(string id)
        {
            s_id = id;
            userKey = id.Replace("@", "").Replace(".", "");

        }
    }
}
