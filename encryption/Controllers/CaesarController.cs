using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class CaesarController : Controller
    {
        //Encryption Action

        [Authorize]
        public IActionResult CaesarEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarEncryption(string plainText, int key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = CaesarEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action

        [Authorize]
        public IActionResult CaesarDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarDecryption(string plainText, int Key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = CaesarDecrypt(plainText, Key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }


        //Caesar Cipher Algorithm

        public static string CaesarEncrypt(string plaintext, int key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ");

            string ciphertext = "";
            foreach (char c in plaintext)
            {
                char shifted = (char)(((int)char.ToUpper(c) + key - 65) % 26 + 65);
                ciphertext += char.IsLower(c) ? char.ToLower(shifted) : shifted;
            }
            return ciphertext;
        }

        public static string CaesarDecrypt(string ciphertext, int key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", " ");

            string plaintext = "";
            foreach (char c in ciphertext)
            {
                char shifted = (char)(((int)char.ToUpper(c) - key - 65 + 26) % 26 + 65);
                plaintext += char.IsLower(c) ? char.ToLower(shifted) : shifted;
            }
            return plaintext;
        }
    }
}
