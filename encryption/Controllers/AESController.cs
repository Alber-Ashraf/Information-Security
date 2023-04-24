using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Packaging;
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
        public IActionResult AESEncryption(string plainText)
        {
            using (AesManaged aes = new AesManaged())
            {
                byte[] encrypted = AESEncrypt(plainText, aes.Key, aes.IV);

                ViewBag.Massage = System.Text.Encoding.UTF8.GetString(encrypted);
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
        public IActionResult AESDecryption(string plainText)
        {
            byte[] cipherText = new byte[plainText.Length]; ;

            int i = 0;
            foreach (char c in plainText)
            {
                cipherText[i] = (byte)c;
                i++;
            }
            using (AesManaged aes = new AesManaged())
            {
                string decrypted = AESDecrypt(cipherText, aes.Key, aes.IV);

                ViewBag.Massage = decrypted;
            }
            return View();
        }

        //AES Chipher Algorithm

        static byte[] AESEncrypt(string plainText, byte[] Key, byte[] IV)
        {
            byte[] encrypted;
            // Create a new AesManaged.
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream
                    // to encrypt
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            // Return encrypted data
            return encrypted;
        }
        static string AESDecrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
