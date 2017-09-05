using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ycombinator.data
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            RNGCryptoServiceProvider providor = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[10];
            providor.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public static string HashPassword(string Password, string Salt)
        {
            SHA256Managed crypt = new SHA256Managed();
            string combinedString = Password + Salt;
            byte[] combined = Encoding.Unicode.GetBytes(combinedString);

            byte[] hash = crypt.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public static bool PasswordMatch(string userInput, string salt, string PasswordHash)
        {
            string userInputHash = HashPassword(userInput, salt);
            return userInputHash == PasswordHash;
        }

    }
}
