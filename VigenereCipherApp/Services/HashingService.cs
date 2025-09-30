using System.Security.Cryptography;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class HashingService
    {
        public string ComputeHash(string inputText, string algorithm)
        {
            using HashAlgorithm hashAlg = algorithm.ToUpper() switch
            {
                "SHA256" => SHA256.Create(),
                "SHA1" => SHA1.Create(),
                "SHA384" => SHA384.Create(),
                "SHA512" => SHA512.Create(),
                "MD5" => MD5.Create(),
                _ => SHA256.Create()
            };

            byte[] bytes = Encoding.UTF8.GetBytes(inputText);
            byte[] hash = hashAlg.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public bool CompareHashes(string hash1, string hash2) =>
            hash1.Trim().ToLower() == hash2.Trim().ToLower();
    }
}
