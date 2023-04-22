using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace encryption.Controllers
{
    public class PlayfairController : Controller
    {
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

        public static string PlayfairEncrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            return Playfair(plaintext, key, true);
        }

        public static string PlayfairDecrypt(string plaintext, string key)
        {
            // Remove any non-letter characters from the plaintext and convert to uppercase
            plaintext = Regex.Replace(plaintext, "[^A-Za-z]+", " ").ToUpper();

            // Remove any non-letter characters from the key and convert to uppercase
            key = Regex.Replace(key, "[^A-Za-z]+", " ").ToUpper();

            return Playfair(plaintext, key, false);
        }
    }
}
