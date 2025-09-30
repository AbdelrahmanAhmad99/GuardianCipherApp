using Microsoft.AspNetCore.Mvc;
using VigenereCipherApp.Services;

namespace VigenereCipherApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly VigenereCipherService _cipherService = new();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EncryptDecrypt(string inputText, string key, string mode)
        {
            if (string.IsNullOrEmpty(inputText) || string.IsNullOrEmpty(key))
                return RedirectToAction("Index");

            string result = mode == "encrypt" ? _cipherService.Encrypt(inputText, key) : _cipherService.Decrypt(inputText, key);
            ViewBag.Result = result;
            return View("Index");
        }
    }
}
