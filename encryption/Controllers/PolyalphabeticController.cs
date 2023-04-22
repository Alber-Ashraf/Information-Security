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
        public IActionResult PolyalphabeticEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = PolyalphabeticEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        public IActionResult PolyalphabeticDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PolyalphabeticDecryption(string plainText, string Key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = PolyalphabeticDecrypt(plainText, Key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        public static string PolyalphabeticEncrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            // Generate the key matrix based on the key
            int[,] keyMatrix = GenerateKeyMatrix(key);

            // Encrypt the plaintext using the key matrix
            string ciphertext = "";
            int keySize = key.Length;
            for (int i = 0; i < plaintext.Length; i++)
            {
                if (plaintext[i] == ' ')
                {
                    ciphertext += ' ';
                }
                else
                {
                    char c = plaintext[i];
                    int rowIndex = i % keySize;
                    int colIndex = c - 'A';
                    int encrypted = keyMatrix[rowIndex, colIndex];
                    ciphertext += (char)(encrypted + 'A');
                }  
            }

            return ciphertext;
        }

        public static string PolyalphabeticDecrypt(string ciphertext, string key)
        {
            // Remove any non-letter characters from the ciphertext and convert to uppercase
            ciphertext = Regex.Replace(ciphertext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            // Generate the key matrix based on the key
            int[,] keyMatrix = GenerateKeyMatrix(key);

            // Decrypt the ciphertext using the key matrix
            string plaintext = "";
            int keySize = key.Length;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (ciphertext[i] == ' ')
                {
                    plaintext += ' ';
                }
                else
                {
                    char c = ciphertext[i];
                    int rowIndex = i % keySize;
                    int[] row = GetRow(keyMatrix, rowIndex);
                    int decrypted = Array.IndexOf(row, c - 'A');
                    plaintext += (char)(decrypted + 'A');
                } 
            }
            return plaintext;
        }

        private static int[,] GenerateKeyMatrix(string key)
        {
            int keySize = key.Length;
            int[,] keyMatrix = new int[keySize, 26];
            for (int i = 0; i < keySize; i++)
            {
                int shift = key[i] - 'A';
                for (int j = 0; j < 26; j++)
                {
                    keyMatrix[i, j] = (j + shift) % 26;
                }
            }
            return keyMatrix;
        }

        private static int[] GetRow(int[,] matrix, int rowIndex)
        {
            int[] row = new int[matrix.GetLength(1)];
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                row[j] = matrix[rowIndex, j];
            }
            return row;
        }
    }
}
