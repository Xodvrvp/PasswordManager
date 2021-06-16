using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerCore
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword(int passwordLenght, bool lowerCase, bool upperCase, bool digits, bool special)
        {
            if (!lowerCase && !upperCase && !digits && !special) throw new ArgumentException("Choose characters");
            if (passwordLenght < 1) throw new ArgumentException("Can't create password of zero or less characters");
            StringBuilder sb = new StringBuilder();
            if (lowerCase) sb.Append("qwertyuiopasdfghjklzxcvbnm");
            if (upperCase) sb.Append("QWERTYUIOPASDFGHJKLZXCVBNM");
            if (digits) sb.Append("0123456789");
            if (special) sb.Append("!@#$%^&*()+=,.");
            string characters = sb.ToString();
            StringBuilder password = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (passwordLenght-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    password.Append(characters[(int)(num % (uint)characters.Length)]);
                }
            }
            return password.ToString();
        }
    }
}
