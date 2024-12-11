using System.Security.Cryptography;
using System;
using System.Text;

namespace Application.Extensions
{
    public static class CryptoExtension
    {
        public static string CalculateSHA1Hash(string input) 
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
