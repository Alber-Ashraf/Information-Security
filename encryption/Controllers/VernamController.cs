using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class VernamController : Controller
    {
        //Encryption Action
        public IActionResult VernamEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VernamEncryption(string plainText, string key)
        {
            if (Regex.IsMatch(plainText, "^[01]+$") && Regex.IsMatch(key, "^[01]+$"))
            {
                ViewBag.Massage = VernamEncrypt(plainText, RepeatKey(key,plainText.Length));

            }
            else
                ViewBag.Massage = "Invalid! The Plaintext and key must be in binary state";

            return View();
        }

        //Decryption Action
        public IActionResult VernamDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VernamDecryption(string ciphertext, string key)
        {
            if (Regex.IsMatch(ciphertext, "^[01]+$") && Regex.IsMatch(key, "^[01]+$"))
            {
                ViewBag.Massage = VernamDecrypt(ciphertext, RepeatKey(key, ciphertext.Length));

            }
            else
                ViewBag.Massage = "Invalid! The Plaintext and key must be in binary state";

            return View();
        }

        //Vernam Chipher Algorithm

        public static string VernamEncrypt(string plaintext, string key)
        {
            string ciphertext = "";

            // Convert plaintext and key to binary arrays
            bool[] plainBits = plaintext.Select(c => c == '1').ToArray();
            bool[] keyBits = key.Select(c => c == '1').ToArray();

            // Repeat key to match length of plaintext
            int keyIndex = 0;

            for (int i = 0; i < keyBits.Length && keyIndex < plainBits.Length; i++)
            {
                bool cipherBit = plainBits[keyIndex] ^ keyBits[i];
                ciphertext += cipherBit ? "1" : "0";
                keyIndex++;
            }

            return ciphertext;
        }

        public static string VernamDecrypt(string ciphertext, string key)
        {
            string plaintext = "";

            // Convert ciphertext and key to binary arrays
            bool[] cipherBits = ciphertext.Select(c => c == '1').ToArray();
            bool[] keyBits = key.Select(c => c == '1').ToArray();

            // Repeat key to match length of ciphertext
            int keyIndex = 0;

            for (int i = 0; i < keyBits.Length && keyIndex < cipherBits.Length; i++)
            {
                bool plainBit = cipherBits[keyIndex] ^ keyBits[i];
                plaintext += plainBit ? "1" : "0";
                keyIndex++;
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
