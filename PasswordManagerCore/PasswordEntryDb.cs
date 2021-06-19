using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Xml.Serialization;

/// <summary>
/// Allows saving/loading password entries to/from AES encrypted XML
/// </summary>
namespace PasswordManagerCore
{
    public static class PasswordEntryDb
    {
        /// <summary>
        /// Creates encrypted XML containing password entries
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="passwords"></param>
        /// <param name="password"></param>
        public static void EncryptAndSerialize(string filePath, ObservableCollection<PasswordEntryModel> passwords, SecureString password)
        {
            using (FileStream fs = File.Open(filePath, FileMode.Create))
            {
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {                   
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    byte[] iv = aes.IV;
                    // writes initialization vector at the beggining of the file
                    fs.Write(iv, 0, iv.Length);
                    byte[] salt = new byte[16];
                    // randomly generates salt
                    using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(salt);
                    }
                    // writes salt after iv
                    fs.Write(salt, 0, salt.Length);
                    // creates AES key from password and generated salt
                    aes.Key = PasswordEntryDb.KeyDerivation(password, salt);
                    // serialize password entries from collection
                    using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        XmlSerializer xmlser = new XmlSerializer(typeof(ObservableCollection<PasswordEntryModel>));
                        xmlser.Serialize(cs, passwords);
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts and returns list of password entries from encrypted XML
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ObservableCollection<PasswordEntryModel> DecryptAndDeserialize(string filePath, SecureString password)
        {
            using (FileStream fs = File.Open(filePath, FileMode.Open))
            {
                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {                   
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    byte[] iv = new byte[aes.IV.Length];
                    // reads iv from the beginning of the file
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fs.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    // reads salt
                    byte[] salt = new byte[16];
                    numBytesToRead = salt.Length;
                    numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        int n = fs.Read(salt, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    // creates key from password and loaded salt
                    aes.Key = PasswordEntryDb.KeyDerivation(password, salt);
                    // loads password entries from XML to collection
                    using (CryptoStream cs = new CryptoStream(fs, aes.CreateDecryptor(aes.Key, iv), CryptoStreamMode.Read))
                    {
                        XmlSerializer xmlser = new XmlSerializer(typeof(ObservableCollection<PasswordEntryModel>));
                        return (ObservableCollection<PasswordEntryModel>)xmlser.Deserialize(cs);
                    }
                }
            }
        }

        /// <summary>
        /// Key derivation function. Creates valid AES key from password of any lenght using SHA256 hash.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public static byte[] KeyDerivation(SecureString password, byte[] salt, int iterations = 10000)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (!(password.Length > 0)) throw new ArgumentException("No password provided.");
            // copies  SecureString content to byte array
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            byte[] passwordByteArray = null;
            try
            {
                int length = Marshal.ReadInt32(ptr, -4);
                passwordByteArray = new byte[length];
                GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        passwordByteArray[i] = Marshal.ReadByte(ptr, i);
                    }
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(passwordByteArray, salt, iterations, HashAlgorithmName.SHA256);
                    byte[] keyBytes = key.GetBytes(16);
                    return keyBytes;
                }
                // clears from memory
                finally
                {
                    Array.Clear(passwordByteArray, 0, passwordByteArray.Length);
                    handle.Free();
                }
            }
            // clears from memory
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}
