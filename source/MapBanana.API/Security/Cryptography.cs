using System;
using System.Security.Cryptography;
using System.Text;

namespace MapBanana.API.Security
{
    public class Cryptography
    {
        public static string Hash64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = SHA256.HashData(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
