using Microsoft.AspNetCore.Mvc;
using VigenereCipherApp.Services;

namespace VigenereCipherApp.Controllers
{
    public class RsaController : Controller
    {
        private readonly RsaEncryptionService _rsaService;

        public RsaController()
        {
            _rsaService = new RsaEncryptionService();
        }

        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Encrypt(string inputText, string publicKey)
        {
            try
            {
                var result = _rsaService.Encrypt(inputText, publicKey);
                TempData["Result"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Encryption failed: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Decrypt(string encryptedText, string privateKey)
        {
            try
            {
                var result = _rsaService.Decrypt(encryptedText, privateKey);
                TempData["Result"] = result;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Decryption failed: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GenerateKeys()
        {
            var (publicKey, privateKey) = _rsaService.GenerateKeys();
            TempData["PublicKey"] = publicKey;
            TempData["PrivateKey"] = privateKey;
            TempData["Info"] = "Keys generated successfully.";
            return RedirectToAction("Index");
        }
    }
}
