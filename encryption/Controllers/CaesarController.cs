using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class CaesarController : Controller
    {
        //Encryption Action
        public IActionResult CaesarEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarEncryption(string plainText, int key)
        {

            if (key >= 0)
            {
                ViewBag.Massage = CaesarEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid! The key must be a positive number";

            return View();
        }

        //Decryption Action

        public IActionResult CaesarDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarDecryption(string ciphertext, int key)
        {
            if (key >= 0)
            {
                ViewBag.Massage = CaesarDecrypt(ciphertext, key);

            }
            else
                ViewBag.Massage = "Invalid! The key must be a positive number";

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
