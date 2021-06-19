using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManagerCore
{
    /// <summary>
    /// Creates randomly generated passwords
    /// </summary>
    public static class PasswordGenerator
    {
        /// <summary>
        /// Randomly generates password of selected lenght and containing selected sets of symbols.
        /// </summary>
        /// <param name="passwordLenght"></param>
        /// <param name="lowerCase"></param>
        /// <param name="upperCase"></param>
        /// <param name="digits"></param>
        /// <param name="special"></param>
        /// <returns></returns>
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
