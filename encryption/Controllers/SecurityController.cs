using encryption.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace encryption.Controllers
{
    public class SecurityController : Controller
    {
        public IActionResult Encryption()
        {
            return View();
        }

        public IActionResult Decryption()
        {
            return View();
        }

        // Caesar Action
        //Encryption Action
        public IActionResult CaesarEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarEncryption(string plainText, int key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = CaesarEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        public IActionResult CaesarDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CaesarDecryption(string StringName, int Key)
        {
            if (!String.IsNullOrEmpty(StringName))
            {
                ViewBag.Massage = CaesarDecrypt(StringName, Key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        // Monoalphabetic Action
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

        // Playfair Action
        //Encryption Action
        public IActionResult PlayfairEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlayfairEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = PlayfairEncrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //Decryption Action
        public IActionResult PlayfairDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlayfairDecryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = PlayfairDecrypt(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        // HillCipher Action
        //Encryption Action
        public IActionResult HillCipherEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult HillCipherEncryption(string plainText, string key)
        {
            if (!String.IsNullOrEmpty(plainText))
            {
                ViewBag.Massage = HillCipher(plainText, key);

            }
            else
                ViewBag.Massage = "Invalid";

            return View();
        }

        //------------------------------------------------------
        //Caesar Cipher Algorithm
        public static char Caesarcipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);
        }

        public static string CaesarEncrypt(string input, int key)
        {
            string output = string.Empty;

            foreach (char ch in input)
                output += Caesarcipher(ch, key);

            return output;
        }

        public static string CaesarDecrypt(string input, int key)
        {
            return CaesarEncrypt(input, 26 - key);
        }

        // Monoalphabetic Cipher Algorithm
        static string MonoalphabeticEncrypt(string plainText, string key)
        {
            char[] chars = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] == ' ')
                {
                    chars[i] = ' ';
                }

                else
                {
                    int j = plainText[i] - 97;
                    chars[i] = key[j];
                }
            }

            return new string(chars);
        }

        static string MonoalphabeticDecrypt(string cipherText, string key)
        {
            char[] chars = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] == ' ')
                {
                    chars[i] = ' ';
                }
                else
                {
                    int j = key.IndexOf(cipherText[i]) + 97;
                    chars[i] = (char)j;
                }
            }
            return new string(chars);
        }
        //Playfair Cipher Algorithm
        private static int PlayfairMod(int a, int b)
        {
            return (a % b + b) % b;
        }

        private static List<int> PlayfairFindAllOccurrences(string str, char value)
        {
            List<int> indexes = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(value, index)) != -1)
                indexes.Add(index++);

            return indexes;
        }

        private static string PlayfairRemoveAllDuplicates(string str, List<int> indexes)
        {
            string retVal = str;

            for (int i = indexes.Count - 1; i >= 1; i--)
                retVal = retVal.Remove(indexes[i], 1);

            return retVal;
        }

        private static char[,] PlayfairGenerateKeySquare(string key)
        {
            char[,] keySquare = new char[5, 5];
            string defaultKeySquare = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            string tempKey = string.IsNullOrEmpty(key) ? "CIPHER" : key.ToUpper();

            tempKey = tempKey.Replace("J", "");
            tempKey += defaultKeySquare;

            for (int i = 0; i < 25; ++i)
            {
                List<int> indexes = PlayfairFindAllOccurrences(tempKey, defaultKeySquare[i]);
                tempKey = PlayfairRemoveAllDuplicates(tempKey, indexes);
            }

            tempKey = tempKey.Substring(0, 25);

            for (int i = 0; i < 25; ++i)
                keySquare[(i / 5), (i % 5)] = tempKey[i];

            return keySquare;
        }

        private static void PlayfairGetPosition(ref char[,] keySquare, char ch, ref int row, ref int col)
        {
            if (ch == 'J')
                PlayfairGetPosition(ref keySquare, 'I', ref row, ref col);

            for (int i = 0; i < 5; ++i)
                for (int j = 0; j < 5; ++j)
                    if (keySquare[i, j] == ch)
                    {
                        row = i;
                        col = j;
                    }
        }

        private static char[] PlayfairSameRow(ref char[,] keySquare, int row, int col1, int col2, int encipher)
        {
            return new char[] { keySquare[row, PlayfairMod((col1 + encipher), 5)], keySquare[row, PlayfairMod((col2 + encipher), 5)] };
        }

        private static char[] PlayfairSameColumn(ref char[,] keySquare, int col, int row1, int row2, int encipher)
        {
            return new char[] { keySquare[PlayfairMod((row1 + encipher), 5), col], keySquare[PlayfairMod((row2 + encipher), 5), col] };
        }

        private static char[] PlayfairSameRowColumn(ref char[,] keySquare, int row, int col, int encipher)
        {
            return new char[] { keySquare[PlayfairMod((row + encipher), 5), PlayfairMod((col + encipher), 5)], keySquare[PlayfairMod((row + encipher), 5), PlayfairMod((col + encipher), 5)] };
        }

        private static char[] PlayfairDifferentRowColumn(ref char[,] keySquare, int row1, int col1, int row2, int col2)
        {
            return new char[] { keySquare[row1, col2], keySquare[row2, col1] };
        }

        private static string PlayfairRemoveOtherChars(string input)
        {
            string output = input;

            for (int i = 0; i < output.Length; ++i)
                if (!char.IsLetter(output[i]))
                    output = output.Remove(i, 1);

            return output;
        }

        private static string PlayfairAdjustOutput(string input, string output)
        {
            StringBuilder retVal = new StringBuilder(output);

            for (int i = 0; i < input.Length; ++i)
            {
                if (!char.IsLetter(input[i]))
                    retVal = retVal.Insert(i, input[i].ToString());

                if (char.IsLower(input[i]))
                    retVal[i] = char.ToLower(retVal[i]);
            }

            return retVal.ToString();
        }

        private static string Playfair(string input, string key, bool encipher)
        {
            string retVal = string.Empty;
            char[,] keySquare = PlayfairGenerateKeySquare(key);
            string tempInput = PlayfairRemoveOtherChars(input);
            int e = encipher ? 1 : -1;

            if ((tempInput.Length % 2) != 0)
                tempInput += "X";

            for (int i = 0; i < tempInput.Length; i += 2)
            {
                int row1 = 0;
                int col1 = 0;
                int row2 = 0;
                int col2 = 0;

                PlayfairGetPosition(ref keySquare, char.ToUpper(tempInput[i]), ref row1, ref col1);
                PlayfairGetPosition(ref keySquare, char.ToUpper(tempInput[i + 1]), ref row2, ref col2);

                if (row1 == row2 && col1 == col2)
                {
                    retVal += new string(PlayfairSameRowColumn(ref keySquare, row1, col1, e));
                }
                else if (row1 == row2)
                {
                    retVal += new string(PlayfairSameRow(ref keySquare, row1, col1, col2, e));
                }
                else if (col1 == col2)
                {
                    retVal += new string(PlayfairSameColumn(ref keySquare, col1, row1, row2, e));
                }
                else
                {
                    retVal += new string(PlayfairDifferentRowColumn(ref keySquare, row1, col1, row2, col2));
                }
            }

            retVal = PlayfairAdjustOutput(input, retVal);

            return retVal;
        }

        public static string PlayfairEncrypt(string input, string key)
        {
            return Playfair(input, key, true);
        }

        public static string PlayfairDecrypt(string input, string key)
        {
            return Playfair(input, key, false);
        }

        //Hill Cipher Algorithm
        public static void HillCiphergetKeyMatrix(String key,
                         int[,] keyMatrix)
        {
            int k = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    keyMatrix[i, j] = (key[k]) % 65;
                    k++;
                }
            }
        }

        public static void HillCipherEncrypt(int[,] cipherMatrix,
                            int[,] keyMatrix,
                            int[,] messageVector)
        {
            int x, i, j;
            for (i = 0; i < 3; i++)
            {
                for (j = 0; j < 1; j++)
                {
                    cipherMatrix[i, j] = 0;

                    for (x = 0; x < 3; x++)
                    {
                        cipherMatrix[i, j] += keyMatrix[i, x] *
                                              messageVector[x, j];
                    }

                    cipherMatrix[i, j] = cipherMatrix[i, j] % 26;
                }
            }
        }

        public static string HillCipher(String message, String key)
        {

            int[,] keyMatrix = new int[3, 3];
            HillCiphergetKeyMatrix(key, keyMatrix);

            int[,] messageVector = new int[3, 1];

            for (int i = 0; i < 3; i++)
                messageVector[i, 0] = (message[i]) % 65;

            int[,] cipherMatrix = new int[3, 1];

            HillCipherEncrypt(cipherMatrix, keyMatrix, messageVector);

            String CipherText = "";

            for (int i = 0; i < 3; i++)
                CipherText += (char)(cipherMatrix[i, 0] + 65);

            return CipherText;
        }

    }
}
