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
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", "").ToUpper();

            string ciphertext = "";
            foreach (char c in plaintext)
            {
                char encryptedChar = (char)((((int)c - 'A' + key) % 26) + 'A');
                ciphertext += encryptedChar;
            }
            return ciphertext;
        }

        public static string CaesarDecrypt(string ciphertext, int key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", "").ToUpper();

            string plaintext = "";
            foreach (char c in ciphertext)
            {
                char decryptedChar = (char)((((int)c - 'A' - key + 26) % 26) + 'A');
                plaintext += decryptedChar;
            }
            return plaintext;
        }
    }
}
