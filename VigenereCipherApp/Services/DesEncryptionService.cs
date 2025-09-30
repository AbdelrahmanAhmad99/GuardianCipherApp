using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class DesEncryptionService : ICryptoService
    {
        public string Encrypt(string plainText, string key)
        {
            using (DES des = DES.Create())
            {
                des.Key = GetValidKey(key);
                des.GenerateIV();
                using (var encryptor = des.CreateEncryptor(des.Key, des.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(des.IV, 0, des.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] data = Convert.FromBase64String(cipherText);
            using (DES des = DES.Create())
            {
                des.Key = GetValidKey(key);
                byte[] iv = new byte[des.BlockSize / 8];
                Array.Copy(data, iv, iv.Length);
                des.IV = iv;
                using (var decryptor = des.CreateDecryptor(des.Key, des.IV))
                using (var ms = new MemoryStream(data, iv.Length, data.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private byte[] GetValidKey(string key)
        {
            return Encoding.UTF8.GetBytes(key.PadRight(8).Substring(0, 8));
        }
    }
}
