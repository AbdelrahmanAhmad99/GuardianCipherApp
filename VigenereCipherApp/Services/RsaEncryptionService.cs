using System;
using System.Security.Cryptography;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class RsaEncryptionService : IAsymmetricCryptoService
    {
        public string Encrypt(string plainText, string publicKeyXml)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXml);

            var bytesToEncrypt = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = rsa.Encrypt(bytesToEncrypt, false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedText, string privateKeyXml)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKeyXml);

            var bytesToDecrypt = Convert.FromBase64String(encryptedText);
            var decryptedBytes = rsa.Decrypt(bytesToDecrypt, false);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public (string publicKey, string privateKey) GenerateKeys()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            return (rsa.ToXmlString(false), rsa.ToXmlString(true));
        }
    }
}
