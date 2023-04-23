using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class AutokeyController : Controller
    {
        //Encryption Action
        public IActionResult AutokeyEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AutokeyEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = AutokeyEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        public IActionResult AutokeyDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AutokeyDecryption(string plainText, string Key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = AutokeyDecrypt(plainText, Key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Autokey Chipher Algorithm

        public static string AutokeyEncrypt(string plaintext, string key)
        {

            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", "").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", "").ToUpper();

            string ciphertext = "";
            string fullKey = key + plaintext;

            for (int i = 0; i < plaintext.Length; i++)
            {
                int shift = fullKey[i] - 'A';
                char encryptedChar = (char)(((plaintext[i] - 'A' + shift) % 26) + 'A');
                ciphertext += encryptedChar;
            }

            return ciphertext;
        }

        public static string AutokeyDecrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", "").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", "").ToUpper();

            string ciphertext = "";
            string fullKey = key;

            for (int i = 0; i < plaintext.Length; i++)
            {
                int shift = fullKey[i] - 'A';
                char decryptedChar = (char)(((plaintext[i] - 'A' - shift + 26) % 26) + 'A');
                ciphertext += decryptedChar;
                fullKey += decryptedChar;
            }

            return ciphertext;
        }
    }
}
