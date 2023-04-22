using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class MonoalphabeticController : Controller
    {
        //Encryption Action
        public IActionResult MonoalphabeticEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MonoalphabeticEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = MonoalphabeticEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        public IActionResult MonoalphabeticDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MonoalphabeticDecryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = MonoalphabeticDecrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }
        //Monoalphabetic Cipher Algorithm
        public static string MonoalphabeticEncrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            // Generate the substitution alphabet based on the key
            char[] substitutionAlphabet = GenerateSubstitutionAlphabet(key);

            // Encrypt the plaintext using the substitution alphabet
            string ciphertext = "";
            foreach (char c in plaintext)
            {
                if(c == ' ')
                {
                    ciphertext += ' ';
                }
                else if (char.IsLetter(c))
                {
                    int index = c - 'A';
                    char encrypted = substitutionAlphabet[index];
                    ciphertext += char.IsLower(c) ? char.ToLower(encrypted) : encrypted;
                }
                else
                {
                    ciphertext += c;
                }
            }

            return ciphertext;
        }

        public static string MonoalphabeticDecrypt(string ciphertext, string key)
        {
            // Remove any non-letter characters from the ciphertext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            // Generate the substitution alphabet based on the key
            char[] substitutionAlphabet = GenerateSubstitutionAlphabet(key);

            // Decrypt the ciphertext using the substitution alphabet
            string plaintext = "";
            foreach (char c in ciphertext)
            {
                if (char.IsLetter(c))
                {
                    int index = Array.IndexOf(substitutionAlphabet, char.ToUpper(c));
                    char decrypted = (char)('A' + index);
                    plaintext += char.IsLower(c) ? char.ToLower(decrypted) : decrypted;
                }
                else
                {
                    plaintext += c;
                }
            }

            return plaintext;
        }

        private static char[] GenerateSubstitutionAlphabet(string key)
        {
            // Generate the substitution alphabet by copying the standard alphabet and replacing letters with those in the key
            char[] substitutionAlphabet = new char[26];
            for (int i = 0; i < 26; i++)
            {
                substitutionAlphabet[i] = (char)('A' + i);
            }
            int j = 0;
            for (int i = 0; i < key.Length; i++)
            {
                char keyChar = char.ToUpper(key[i]);
                if (substitutionAlphabet[keyChar - 'A'] != keyChar)
                {
                    continue;
                }
                substitutionAlphabet[keyChar - 'A'] = substitutionAlphabet[j];
                substitutionAlphabet[j] = keyChar;
                j++;
            }
            return substitutionAlphabet;
        }
    }
}
