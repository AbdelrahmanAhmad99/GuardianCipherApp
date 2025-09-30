using Microsoft.AspNetCore.Mvc;
using VigenereCipherApp.Services;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VigenereCipherApp.Controllers
{
    public class CryptoController : Controller
    {
        private readonly Dictionary<string, ICryptoService> _cryptoServices;

        public CryptoController()
        {
            _cryptoServices = new()
            {
                { "vigenere", new VigenereCipherService() },
                { "des", new DesEncryptionService() },
                { "aes", new AesEncryptionService() },
                { "tripledes", new TripleDesEncryptionService() }, 
                { "playfair", new PlayfairCipherService() }

            };
        }

        [HttpPost]
        public IActionResult EncryptDecrypt(string inputText, string key, string method, string operation)
        {
            // Validate input for Playfair Cipher (only letters allowed)
            if (method == "playfair" && !IsValidPlayfairInput(inputText))
            {
                TempData["PlayfairError"] = "Playfair Cipher only supports alphabetic characters (A-Z, a-z).";
                return RedirectToAction("Index", "Home");
            }

            if (!_cryptoServices.ContainsKey(method))
            {
                TempData["Error"] = "Unsupported encryption method.";
                return RedirectToAction("Index", "Home");
            }

            var service = _cryptoServices[method];
            string result;

            try
            {
                result = operation == "encrypt"
                    ? service.Encrypt(inputText, key)
                    : service.Decrypt(inputText, key);
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }

            TempData["Result"] = result;
            return RedirectToAction("Index", "Home");
        }

        // Method to validate Playfair Cipher input (only alphabetic characters)
        private bool IsValidPlayfairInput(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z]+$"); // Allow only alphabetic characters
        }
    }
}
