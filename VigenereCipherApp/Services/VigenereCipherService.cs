using System;
using System.Text;

namespace VigenereCipherApp.Services
{
    public class VigenereCipherService : ICryptoService
    {
        private readonly string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string Encrypt(string plaintext, string key) => ProcessText(plaintext, key, true);
        public string Decrypt(string ciphertext, string key) => ProcessText(ciphertext, key, false);

        private string ProcessText(string input, string key, bool encrypt)
        {
            StringBuilder result = new();
            key = key.ToUpper();
            int keyIndex = 0;

            foreach (char c in input.ToUpper())
            {
                if (_alphabet.Contains(c))
                {
                    int shift = (_alphabet.IndexOf(key[keyIndex]) + 1) * (encrypt ? 1 : -1);
                    int newIndex = (_alphabet.IndexOf(c) + shift + _alphabet.Length) % _alphabet.Length;
                    result.Append(_alphabet[newIndex]);

                    keyIndex = (keyIndex + 1) % key.Length;
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}

 
 
