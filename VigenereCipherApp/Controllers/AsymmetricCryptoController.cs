using Microsoft.AspNetCore.Mvc;
using VigenereCipherApp.Services;

namespace VigenereCipherApp.Controllers
{
    public class AsymmetricCryptoController : Controller
    {
        private readonly Dictionary<string, IAsymmetricCryptoService> _cryptoServices;

        public AsymmetricCryptoController()
        {
            _cryptoServices = new()
            {
                { "rsa", new RsaEncryptionService() },
                { "elgamal", new ElGamalEncryptionService() }
            };
        }

        [HttpPost]
        public IActionResult Encrypt(string inputText, string publicKey, string method)
        {
            try
            {
                var service = _cryptoServices[method];
                string result = service.Encrypt(inputText, publicKey);
                TempData["Result"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Encryption Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrypt(string encryptedText, string privateKey, string method)
        {
            try
            {
                var service = _cryptoServices[method];
                string result = service.Decrypt(encryptedText, privateKey);
                TempData["Result"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Decryption Error: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GenerateKeys(string method)
        {
            var service = _cryptoServices[method];
            var (publicKey, privateKey) = service.GenerateKeys();

            TempData["PublicKey"] = publicKey;
            TempData["PrivateKey"] = privateKey;
            TempData["Info"] = "Keys generated successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Index() => View();
    }
}
