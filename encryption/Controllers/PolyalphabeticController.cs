using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class PolyalphabeticController : Controller
    {

        //Encryption Action
        public IActionResult PolyalphabeticEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PolyalphabeticEncryption(string plainText)
        {
            ViewBag.Massage = PolyalphabeticEncrypt(plainText);

            return View();
        }

        //Decryption Action

        public IActionResult PolyalphabeticDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PolyalphabeticDecryption(string ciphertext)
        {
            ViewBag.Massage = PolyalphabeticDecrypt(ciphertext);

            return View();
        }

        public static string PolyalphabeticEncrypt(string plaintext)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", "").ToUpper();

            string ciphertext = "";
            int shift1 = 3;
            int shift2 = 5;
            int shift3 = 7;

            for (int i = 0; i < plaintext.Length; i += 3)
            {
                if (i + 2 < plaintext.Length)
                {
                    char char1 = plaintext[i];
                    char char2 = plaintext[i + 1];
                    char char3 = plaintext[i + 2];

                    char1 = (char)((char1 - 'A' + shift1) % 26 + 'A');
                    char2 = (char)((char2 - 'A' + shift2) % 26 + 'A');
                    char3 = (char)((char3 - 'A' + shift3) % 26 + 'A');

                    ciphertext += char1.ToString() + char2.ToString() + char3.ToString();
                }
                else if (i + 1 < plaintext.Length)
                {
                    // If there are only two characters left, pad with 'x'
                    char char1 = plaintext[i];
                    char char2 = plaintext[i + 1];

                    char1 = (char)((char1 - 'A' + shift1) % 26 + 'A');
                    char2 = (char)((char2 - 'A' + shift2) % 26 + 'A');

                    ciphertext += char1.ToString() + char2.ToString();
                }
                else
                {
                    // If there is only one character left, pad with 'x'
                    char char1 = plaintext[i];

                    char1 = (char)((char1 - 'A' + shift1) % 26 + 'A');

                    ciphertext += char1.ToString();
                }
            }

            return ciphertext;
        }

        public static string PolyalphabeticDecrypt(string ciphertext)
        {
            // Remove any non-letter characters from the ciphertext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", "").ToUpper();

            string plaintext = "";
            int shift1 = 3;
            int shift2 = 5;
            int shift3 = 7;

            for (int i = 0; i < ciphertext.Length; i += 3)
            {
                if (i + 2 < ciphertext.Length)
                {
                    char char1 = ciphertext[i];
                    char char2 = ciphertext[i + 1];
                    char char3 = ciphertext[i + 2];

                    char1 = (char)((char1 - 'A' + 26 - shift1) % 26 + 'A');
                    char2 = (char)((char2 - 'A' + 26 - shift2) % 26 + 'A');
                    char3 = (char)((char3 - 'A' + 26 - shift3) % 26 + 'A');

                    plaintext += char1.ToString() + char2.ToString() + char3.ToString();
                }
                else if (i + 1 < ciphertext.Length)
                {
                    // If there are only two characters left, pad with 'x'
                    char char1 = ciphertext[i];
                    char char2 = ciphertext[i + 1];

                    char1 = (char)((char1 - 'A' + 26 - shift1) % 26 + 'A');
                    char2 = (char)((char2 - 'A' + 26 - shift2) % 26 + 'A');

                    plaintext += char1.ToString() + char2.ToString();
                }
                else
                {
                    // If there is only one character left, pad with 'x'
                    char char1 = ciphertext[i];

                    char1 = (char)((char1 - 'A' + 26 - shift1) % 26 + 'A');

                    plaintext += char1.ToString();
                }
            }

            return plaintext;
        }
    }
}
