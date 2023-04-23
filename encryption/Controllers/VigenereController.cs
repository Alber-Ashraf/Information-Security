using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class VigenereController : Controller
    {

        //Encryption Action
        [Authorize]
        public IActionResult VigenereEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VigenereEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = VigenereEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        [Authorize]
        public IActionResult VigenereDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VigenereDecryption(string plainText, string Key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = VigenereDecrypt(plainText, Key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Vigenere Chipher Algorithm

        public static string VigenereEncrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            string ciphertext = "";
            int keywordIndex = 0;

            foreach (char c in plaintext)
            {
                if (c == ' ')
                {
                    ciphertext += ' ';
                }
                else
                {
                    int shift = key[keywordIndex] - 'A';
                    char encryptedChar = (char)(((c - 'A' + shift) % 26) + 'A');
                    ciphertext += encryptedChar;

                    keywordIndex = (keywordIndex + 1) % key.Length;
                }
            }

            return ciphertext;
        }

        public static string VigenereDecrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            string ciphertext = "";
            int keywordIndex = 0;

            foreach (char c in plaintext)
            {
                if (c == ' ')
                {
                    ciphertext += ' ';
                }
                else
                {
                    int shift = key[keywordIndex] - 'A';
                    char decryptedChar = (char)(((c - 'A' - shift + 26) % 26) + 'A');
                    ciphertext += decryptedChar;

                    keywordIndex = (keywordIndex + 1) % key.Length;
                }
                
            }

            return ciphertext;
        }
    }
}
