using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TicketingSystem.Utils
{
    public class EncryptionHelper
    {
        public string Encrypt(string key, string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException(nameof(data));

            byte[][] keys = GetHashKeys(key);
            return EncryptStringToBytes_Aes(data, keys[0], keys[1]);
        }

        public string Decrypt(string key, string data)
        {
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException(nameof(data));

            byte[][] keys = GetHashKeys(key);
            return DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
        }

        private byte[][] GetHashKeys(string key)
        {
            using (var sha2 = SHA256.Create())
            {
                byte[] rawKey = Encoding.UTF8.GetBytes(key);
                byte[] hashKey = sha2.ComputeHash(rawKey);
                byte[] hashIV = sha2.ComputeHash(Encoding.UTF8.GetBytes(key));

                // Ensure IV is 16 bytes long (AES block size)
                Array.Resize(ref hashIV, 16);

                return new[] { hashKey, hashIV };
            }
        }

        private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var msDecrypt = new MemoryStream(cipherText))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}
