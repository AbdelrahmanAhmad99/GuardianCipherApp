using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class TripleDesEncryptionService : ICryptoService
    {
        public string Encrypt(string plainText, string key)
        {
            using TripleDESCryptoServiceProvider tripleDES = new();
            tripleDES.Key = GetKey(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;

            byte[] data = Encoding.UTF8.GetBytes(plainText);
            using ICryptoTransform transform = tripleDES.CreateEncryptor();
            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(results);
        }

        public string Decrypt(string cipherText, string key)
        {
            using TripleDESCryptoServiceProvider tripleDES = new();
            tripleDES.Key = GetKey(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;

            byte[] data = Convert.FromBase64String(cipherText);
            using ICryptoTransform transform = tripleDES.CreateDecryptor();
            byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(results);
        }

        private byte[] GetKey(string key)
        {
            // TripleDES needs a 24-byte key
            using var sha1 = SHA1.Create();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashed = sha1.ComputeHash(keyBytes);

            byte[] finalKey = new byte[24];
            Array.Copy(hashed, finalKey, 24 > hashed.Length ? hashed.Length : 24);
            return finalKey;
        }
    }
}
