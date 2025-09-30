using Microsoft.AspNetCore.Mvc;
using VigenereCipherApp.Services;

namespace VigenereCipherApp.Controllers
{
    public class HashController : Controller
    {
        private readonly HashingService _hashingService;

        public HashController(HashingService hashingService)
        {
            _hashingService = hashingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateHash(string inputText, string algorithm)
        {
            string hash = _hashingService.ComputeHash(inputText, algorithm);
            TempData["GeneratedHash"] = hash;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Compare(string hash1, string hash2)
        {
            bool match = _hashingService.CompareHashes(hash1, hash2);
            TempData["ComparisonResult"] = match ? "Match" : "Do not match";
            return RedirectToAction("Index");
        }
    }
}
