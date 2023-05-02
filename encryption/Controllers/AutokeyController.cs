using Microsoft.AspNetCore.Authorization;
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
            if (Char.IsLetter(key[0]))
            {
                ViewBag.Massage = AutokeyEncrypt(plainText, key);
            }
            else
                ViewBag.Massage = "Invaild! The key must be a character";

            return View();
        }

        //Decryption Action

        public IActionResult AutokeyDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AutokeyDecryption(string ciphertext, string key)
        {
            if (Char.IsLetter(key[0]))
            {
                ViewBag.Massage = AutokeyDecrypt(ciphertext, key);
            }
            else
                ViewBag.Massage = "Invaild! The key must be a character";
            

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

        public static string AutokeyDecrypt(string chiphertext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            chiphertext = Regex.Replace(chiphertext, "[^A-Za-z]+", "").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", "").ToUpper();

            string plaintext = "";
            string fullKey = key;

            for (int i = 0; i < chiphertext.Length; i++)
            {
                int shift = fullKey[i] - 'A';
                char decryptedChar = (char)(((chiphertext[i] - 'A' - shift + 26) % 26) + 'A');
                plaintext += decryptedChar;
                fullKey += decryptedChar;
            }

            return plaintext;
        }
    }
}
