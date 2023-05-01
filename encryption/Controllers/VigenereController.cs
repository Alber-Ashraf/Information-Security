using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class VigenereController : Controller
    {

        //Encryption Action
        public IActionResult VigenereEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VigenereEncryption(string plainText, string key)
        {
            ViewBag.Massage = VigenereEncrypt(plainText, RepeatKey(key, plainText.Length));

            return View();
        }

        //Decryption Action
        public IActionResult VigenereDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VigenereDecryption(string ciphertext, string key)
        {
            ViewBag.Massage = VigenereDecrypt(ciphertext, RepeatKey(key, ciphertext.Length));

            return View();
        }

        //Vigenere Chipher Algorithm

        public static string VigenereEncrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", "").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", "").ToUpper();

            string ciphertext = "";

            for (int i = 0; i < plaintext.Length; i++)
            {
                char encryptedChar = (char)(((((int)plaintext[i] - 'A') + ((int)key[i] - 'A')) % 26) + 'A');
                ciphertext += encryptedChar;
            }

            return ciphertext;
        }

        public static string VigenereDecrypt(string ciphertext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", "").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", "").ToUpper();

            string plaintext = "";

            for (int i = 0; i < ciphertext.Length; i++)
            {
                char plainChar = (char)(((((int)ciphertext[i] - 'A') - ((int)key[i] - 'A')) + 26) % 26 + 'A');
                plaintext += plainChar;
            }

            return plaintext;
        }

        public static string RepeatKey(string key, int length)
        {
            StringBuilder repeatedKey = new StringBuilder();
            while (repeatedKey.Length < length)
            {
                repeatedKey.Append(key);
            }
            return repeatedKey.ToString().Substring(0, length);
        }
    }
}
