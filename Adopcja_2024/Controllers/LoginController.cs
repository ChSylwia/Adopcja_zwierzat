using Adopcja_2024.Data;
using Adopcja_2024.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Adopcja_2024.Controllers
{
    public class LoginController : Controller
    {
        private readonly MyDbContext _context;

        public LoginController(MyDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["user"] != null)
            {
                string message = (string)TempData["user"];
                ViewData["Message"] = message;
            }
            if (Request.Cookies["CookieUserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Index([Bind("email,haslo")] User user)
        {
            if (ModelState.IsValid)
            {
                MD5 md5 = MD5.Create();
                //var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(HttpContext.Request.Form["haslo"]));
                var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(user.haslo));
                string password = Convert.ToHexString(byte_data);

                user.haslo = password;
                //u = LoginService(HttpContext.Request.Form["email"], password.ToLower());

                var osoba = _context.osoba
                    .Where(o => o.email == user.email && o.haslo == user.haslo)
                    .Select(o => new User
                    {
                        id_osoba = o.id_osoba,
                        email = o.email,
                        haslo = o.haslo
                    })
                    .FirstOrDefault();

                if (osoba != null)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMinutes(60);
                    Response.Cookies.Append("CookieUserID", osoba.id_osoba.ToString(), options);
                    Response.Cookies.Append("CookieUserEmail", osoba.email, options);
                    System.Diagnostics.Debug.WriteLine($"Znaleziono użytkownika: ID={osoba.id_osoba}, Email={osoba.email}");
                }
                else
                {
                    string data = "Logowanie nie powiodło się!";
                    TempData["user"] = data;

                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cookies.Delete("CookieUserID");
            Response.Cookies.Delete("CookieUserName");
            Response.Cookies.Delete("CookieUserEmail");
            return RedirectToAction("Index");
        }

    }
}
