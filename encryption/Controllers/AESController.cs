using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
using System;
using System.Security.Cryptography;
using System.Text;

namespace encryption.Controllers
{
    public class AESController : Controller
    {
        //Encryption Action

        [Authorize]
        public IActionResult AESEncryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AESEncryption(string plainText, string key, string iv)
        {
            if(key.Length == 32 && iv.Length == 16)
            {
                byte[] key1 = Encoding.UTF8.GetBytes(key);
                byte[] iv1 = Encoding.UTF8.GetBytes(iv);

                // Encrypt the original text
                byte[] encrypted = AESEncrypt(plainText, key1, iv1);

                ViewBag.Massage = Convert.ToBase64String(encrypted);
            }
            else
            {
                ViewBag.Massage = "The Key Length must be 32 bytes and IV Length must be 16 bytes";
            }

            return View();
        }

        //Decryption Action

        [Authorize]
        public IActionResult AESDecryption()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AESDecryption(string plainText, string key, string iv)
        {
            if (key.Length == 32 && iv.Length == 16)
            {
                byte[] newKey = Encoding.UTF8.GetBytes(key);
                byte[] newIv = Encoding.UTF8.GetBytes(iv);

                ViewBag.Massage = AESDecrypt(plainText, newKey, newIv);
            }
            else
            {
                ViewBag.Massage = "The Key Length must be 32 bytes and IV Length must be 16 bytes";
            }
            

            return View();
        }

        //AES Chipher Algorithm

        static byte[] AESEncrypt(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length == 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length == 0)
                throw new ArgumentNullException("iv");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        static string AESDecrypt(string encryptedData, byte[] key, byte[] iv)
        {
            // Convert the encrypted data from base64 string to byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            // Create a new instance of AesCryptoServiceProvider
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                // Set the key and IV
                aes.Key = key;
                aes.IV = iv;

                // Create a new instance of MemoryStream to write the decrypted data to
                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    // Create a new instance of CryptoStream to perform the decryption
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        // Write the encrypted data to the CryptoStream
                        csDecrypt.Write(encryptedBytes, 0, encryptedBytes.Length);

                        // Flush the final block of data through the CryptoStream
                        csDecrypt.FlushFinalBlock();

                        // Convert the decrypted data to string
                        string decryptedData = Encoding.UTF8.GetString(msDecrypt.ToArray());

                        return decryptedData;
                    }
                }
            }
        }
    }
}
