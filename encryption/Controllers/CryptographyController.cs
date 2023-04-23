using encryption.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text;

namespace encryption.Controllers
{
    public class CryptographyController : Controller
    {
        [Authorize]
        public IActionResult Encryption()
        {
            return View();
        }

        [Authorize]
        public IActionResult Decryption()
        {
            return View();
        }
    }
}
