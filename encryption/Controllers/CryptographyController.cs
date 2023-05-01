using encryption.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace encryption.Controllers
{
    public class CryptographyController : Controller
    {
        public IActionResult Encryption()
        {
            return View();
        }

        public IActionResult Decryption()
        {
            return View();
        }
    }
}
