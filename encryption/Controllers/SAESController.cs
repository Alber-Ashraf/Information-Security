﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace encryption.Controllers
{
    public class SAESController : Controller
    {
        //Encryption Action
        public IActionResult SAESEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SAESEncryption(string plaintext, string key)
        {
            if (key.Length == 16)
            {
                ViewBag.Massage = SAESEncrypt(plaintext, key);
            }
            else
                ViewBag.Massage = "Invaild! The key must be 16 bytes";

            return View();
        }

        //Decryption Action

        public IActionResult SAESDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SAESDecryption(string ciphertext, string key)
        {
            if (key.Length == 16)
            {
                ViewBag.Massage = SAESDecrypt(ciphertext, key);
            }
            else
                ViewBag.Massage = "Invaild! The key must be 16 bytes";
            

            return View();
        }

        //AES Chipher Algorithm
        private static readonly byte[] Sbox = new byte[]
    {
        0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76,
        0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0,
        0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15,
        0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75,
        0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84,
        0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf,
        0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8,
        0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2,
        0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73,
        0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb,
        0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79,
        0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08,
        0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a,
        0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e,
        0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf,
        0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16
    };
        private static readonly byte[] InvSbox = new byte[]
    {
        0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB,
        0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB,
        0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E,
        0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25,
        0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92,
        0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84,
        0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06,
        0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B,
        0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73,
        0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E,
        0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B,
        0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4,
        0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F,
        0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF,
        0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61,
        0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D
    };

        private static readonly byte[] Rcon = new byte[] {
        0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a,
        0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91, 0x39,
        0x72, 0xe4, 0xd3, 0xbd, 0x61, 0xc2, 0x9f, 0x25, 0x4a, 0x94, 0x33, 0x66, 0xcc, 0x83, 0x1d, 0x3a,
        0x74, 0xe8, 0xcb, 0x8d, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8,
        0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef,
        0xc5, 0x91, 0x39, 0x72, 0xe4, 0xd3, 0xbd, 0x61, 0xc2, 0x9f, 0x25, 0x4a, 0x94, 0x33, 0x66, 0xcc,
        0x83, 0x1d, 0x3a, 0x74, 0xe8, 0xcb, 0x8d, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1b,
        0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3,
        0x7d, 0xfa, 0xef, 0xc5, 0x91, 0x39, 0x72, 0xe4, 0xd3, 0xbd, 0x61, 0xc2, 0x9f, 0x25, 0x4a, 0x94,
        0x33, 0x66, 0xcc, 0x83, 0x1d, 0x3a, 0x74, 0xe8, 0xcb, 0x8d, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20,
        0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63, 0xc6, 0x97, 0x35,
        0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91, 0x39, 0x72, 0xe4, 0xd3, 0xbd, 0x61, 0xc2, 0x9f,
        0x25, 0x4a, 0x94, 0x33, 0x66, 0xcc, 0x83, 0x1d, 0x3a, 0x74, 0xe8, 0xcb, 0x8d, 0x01, 0x02, 0x04,
        0x08, 0x10, 0x20, 0x40, 0x80, 0x1b, 0x36, 0x6c, 0xd8, 0xab, 0x4d, 0x9a, 0x2f, 0x5e, 0xbc, 0x63,
        0xc6, 0x97, 0x35, 0x6a, 0xd4, 0xb3, 0x7d, 0xfa, 0xef, 0xc5, 0x91, 0x39, 0x72, 0xe4, 0xd3, 0xbd,
        0x61, 0xc2, 0x9f, 0x25, 0x4a, 0x94, 0x33, 0x66, 0xcc, 0x83, 0x1d, 0x3a, 0x74, 0xe8, 0xcb, 0x8d
    };
        private static readonly byte[] InvMixColumnMatrix = new byte[]
    {
        0x0E, 0x0B, 0x0D, 0x09,
        0x09, 0x0E, 0x0B, 0x0D,
        0x0D, 0x09, 0x0E, 0x0B,
        0x0B, 0x0D, 0x09, 0x0E
    };

        private static readonly byte[] MixColumnMatrix = new byte[]
        {
        0x02, 0x03, 0x01, 0x01,
        0x01, 0x02, 0x03, 0x01,
        0x01, 0x01, 0x02, 0x03,
        0x03, 0x01, 0x01, 0x02
        };

        private static byte[] ExpandKey(byte[] key)
        {
            byte[] expandedKey = new byte[176];
            Array.Copy(key, 0, expandedKey, 0, 16);

            for (int i = 16; i < 176; i += 4)
            {
                byte[] temp = new byte[4];
                Array.Copy(expandedKey, i - 4, temp, 0, 4);

                if (i % 16 == 0)
                {
                    temp = RotWord(temp);
                    temp = SubWord(temp);
                    temp[0] ^= Rcon[i / 16];
                }
                for (int j = 0; j < 4; j++)
                {
                    expandedKey[i + j] = (byte)(expandedKey[i - 16 + j] ^ temp[j]);
                }
            }

            return expandedKey;
        }

        private static byte[] AddRoundKey(byte[] state, byte[] expandedKey, int round)
        {
            byte[] roundKey = new byte[16];
            Array.Copy(expandedKey, round * 16, roundKey, 0, 16);

            byte[] newState = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                newState[i] = (byte)(state[i] ^ roundKey[i]);
            }

            return newState;
        }

        private static byte[] SubBytes(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                newState[i] = Sbox[state[i]];
            }

            return newState;
        }

        private static byte[] InvSubBytes(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                newState[i] = InvSbox[state[i]];
            }

            return newState;
        }

        private static byte[] ShiftRows(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                newState[i] = state[(i % 4) * 4 + (i / 4)];
            }

            return newState;
        }

        private static byte[] InvShiftRows(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                newState[(i % 4) * 4 + (i / 4)] = state[i];
            }

            return newState;
        }

        private static byte[] MixColumns(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 4; i++)
            {
                byte[] column = new byte[] { state[i], state[i + 4], state[i + 8], state[i + 12] };
                byte[] newColumn = MixColumn(column);
                newState[i] = newColumn[0];
                newState[i + 4] = newColumn[1];
                newState[i + 8] = newColumn[2];
                newState[i + 12] = newColumn[3];
            }

            return newState;
        }

        private static byte[] InvMixColumns(byte[] state)
        {
            byte[] newState = new byte[16];
            for (int i = 0; i < 4; i++)
            {
                byte[] column = new byte[] { state[i], state[i + 4], state[i + 8], state[i + 12] };
                byte[] newColumn = InvMixColumn(column);
                newState[i] = newColumn[0];
                newState[i + 4] = newColumn[1];
                newState[i + 8] = newColumn[2];
                newState[i + 12] = newColumn[3];
            }

            return newState;
        }

        private static byte[] MixColumn(byte[] column)
        {
            byte[] newColumn = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                newColumn[i] = (byte)(
                    MultiplyGF(MixColumnMatrix[i * 4], column[0]) ^
                    MultiplyGF(MixColumnMatrix[i * 4 + 1], column[1]) ^
                    MultiplyGF(MixColumnMatrix[i * 4 + 2], column[2]) ^
                    MultiplyGF(MixColumnMatrix[i * 4 + 3], column[3])
                );
            }

            return newColumn;
        }

        private static byte[] InvMixColumn(byte[] column)
        {
            byte[] newColumn = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                newColumn[i] = (byte)(
                    MultiplyGF(InvMixColumnMatrix[i * 4], column[0]) ^
                    MultiplyGF(InvMixColumnMatrix[i * 4 + 1], column[1]) ^
                    MultiplyGF(InvMixColumnMatrix[i * 4 + 2], column[2]) ^
                    MultiplyGF(InvMixColumnMatrix[i * 4 + 3], column[3])
                );
            }

            return newColumn;
        }

        private static byte MultiplyGF(byte a, byte b)
        {
            byte result = 0;
            byte highBit = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) == 1)
                    result ^= a;
                highBit = (byte)(a & 0x80);
                a <<= 1;
                if (highBit == 0x80)
                    a ^= 0x1B;
                b >>= 1;
            }
            return result;
        }

        private static byte[] RotWord(byte[] word)
        {
            byte[] newWord = new byte[4];
            newWord[0] = word[1];
            newWord[1] = word[2];
            newWord[2] = word[3];
            newWord[3] = word[0];
            return newWord;
        }

        private static byte[] SubWord(byte[] word)
        {
            byte[] newWord = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                newWord[i] = Sbox[word[i]];
            }
            return newWord;
        }

        public static string SAESEncrypt(string plaintext, string key)
        {
            byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
            int paddingLength = 16 - (plaintextBytes.Length % 16);
            byte[] paddedPlaintextBytes = new byte[plaintextBytes.Length + paddingLength];
            Array.Copy(plaintextBytes, paddedPlaintextBytes, plaintextBytes.Length);

            byte[] expandedKey = ExpandKey(Encoding.ASCII.GetBytes(key));
            byte[] state = new byte[16];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < paddedPlaintextBytes.Length; i += 16)
            {
                Array.Copy(paddedPlaintextBytes, i, state, 0, 16);

                state = AddRoundKey(state, expandedKey, 0);
                for (int j = 1; j <= 9; j++)
                {
                    state = SubBytes(state);
                    state = ShiftRows(state);
                    state = MixColumns(state);
                    state = AddRoundKey(state, expandedKey, j);
                }
                state = SubBytes(state);
                state = ShiftRows(state);
                state = AddRoundKey(state, expandedKey, 10);

                sb.Append(BitConverter.ToString(state).Replace("-", ""));
            }

            return sb.ToString();
        }

        public static string SAESDecrypt(string ciphertext, string key)
        {
            byte[] ciphertextBytes = Enumerable.Range(0, ciphertext.Length)
                                               .Where(x => x % 2 == 0)
                                               .Select(x => Convert.ToByte(ciphertext.Substring(x, 2), 16))
                                               .ToArray();

            byte[] expandedKey = ExpandKey(Encoding.ASCII.GetBytes(key));
            byte[] state = new byte[16];

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ciphertextBytes.Length; i += 16)
            {
                Array.Copy(ciphertextBytes, i, state, 0, 16);

                state = AddRoundKey(state, expandedKey, 10);
                state = InvShiftRows(state);
                state = InvSubBytes(state);

                for (int j = 9; j >= 1; j--)
                {
                    state = AddRoundKey(state, expandedKey, j);
                    state = InvMixColumns(state);
                    state = InvShiftRows(state);
                    state = InvSubBytes(state);
                }

                state = AddRoundKey(state, expandedKey, 0);

                int length = state.Length;
                while (length > 0 && state[length - 1] == 0)
                    length--;
                byte[] result = new byte[length];
                Array.Copy(state, result, length);

                sb.Append(Encoding.ASCII.GetString(result));
            }

            return sb.ToString();
        }
    }
}
