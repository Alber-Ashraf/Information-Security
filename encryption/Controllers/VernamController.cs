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
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plainText = Regex.Replace(plainText, "[^A-Za-z]+", " ");

            if (Regex.IsMatch(key, "^[01]+$"))
            {
                ViewBag.Massage = VernamEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid! The Key must be in binary state";

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
                ViewBag.Massage = VernamDecrypt(ciphertext, key);

            }
            else
                ViewBag.Massage = "Invalid! The ciphertext and the Key must be in binary state";

            return View();
        }

        //Vernam Chipher Algorithm

        public static string VernamEncrypt(string plaintext, string key)
        {
            string ciphertext = "";

            // Convert plaintext and key to binary arrays
            bool[] plainBits = StringToBinary(plaintext).Select(c => c == '1').ToArray();

            string fullkey = RepeatKey(key, plainBits.Length);
            bool[] keyBits = fullkey.Select(c => c == '1').ToArray();

            for (int i = 0; i < plainBits.Length; i++)
            {
                bool cipherBit = plainBits[i] ^ keyBits[i];
                ciphertext += cipherBit ? "1" : "0";
            }

            return ciphertext;
        }

        public static string VernamDecrypt(string ciphertext, string key)
        {
            string plaintext = "";

            // Convert ciphertext and key to binary arrays
            bool[] cipherBits = ciphertext.Select(c => c == '1').ToArray();

            string fullkey = RepeatKey(key, cipherBits.Length);
            bool[] keyBits = fullkey.Select(c => c == '1').ToArray();


            for (int i = 0; i < cipherBits.Length; i++)
            {
                bool plainBit = cipherBits[i] ^ keyBits[i];
                plaintext += plainBit ? "1" : "0";
            }

            return BinaryToString(plaintext);
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

        static string StringToBinary(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            StringBuilder binary = new StringBuilder();
            foreach (byte b in bytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    char c = ((b >> i) & 1) == 1 ? '1' : '0';
                    binary.Append(c);
                }
            }
            return binary.ToString();
        }

        static string BinaryToString(string binary)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < binary.Length; i += 8)
            {
                string binaryChar = binary.Substring(i, 8);
                byte b = Convert.ToByte(binaryChar, 2);
                byteList.Add(b);
            }
            return Encoding.ASCII.GetString(byteList.ToArray());
        }
    }
}
