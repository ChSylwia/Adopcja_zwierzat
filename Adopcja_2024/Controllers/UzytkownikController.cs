using Adopcja_2024.Data;
using Adopcja_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Adopcja_2024.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly MyDbContext _context;
        public UzytkownikController(MyDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {

                return View();
            }
        }

        public async Task<ActionResult> Ogloszenia()
        {
            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<Ogloszenie> ogloszenia = new List<Ogloszenie>();
                ogloszenia = await _context.ogloszenie
                    .Where(o => o.id_osoba == Convert.ToInt32(Request.Cookies["CookieUserID"]))
                    .Select(o => new Ogloszenie
                    {
                        id_ogloszenie = o.id_ogloszenie,
                        tytul = o.tytul,
                        data = o.data,
                        opis = o.opis,
                        zdjecie = o.zdjecie != null ? o.zdjecie : null
                    })
                    .OrderByDescending(o => o.id_ogloszenie)
                    .ToListAsync();
                return View(ogloszenia);
            }
        }

        public async Task<ActionResult> Zwierzaki()
        {
            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<Zwierzak> zwierzaki = _context.zwierzak
                    .Where(z => z.id_osoby == Convert.ToInt32(Request.Cookies["CookieUserID"]))
                    .Include(z => z.Rasa)
                        .ThenInclude(r => r.Gatunek)
                    .Include(z => z.Wiek)
                    .Include(z => z.Wielkosc)
                    .OrderByDescending(z => z.id_zwierzak)
                    .ToList();

                return View(zwierzaki);
            }
        }

        public async Task<ActionResult> Rezerwacja()
        {
            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                List<Zwierzak> zwierzaki = _context.zwierzak
                    .Include(z => z.Rezerwacja)
                    .Include(z => z.Rasa)
                        .ThenInclude(r => r.Gatunek)
                    .Include(z => z.Wiek)
                    .Include(z => z.Wielkosc)
                    .Where(z => z.Rezerwacja.id_osoba == Convert.ToInt32(Request.Cookies["CookieUserID"]))
                    .OrderBy(z => z.id_zwierzak)
                    .ToList();

                return View(zwierzaki);
            }
        }

        public async Task<ActionResult> RezerwacjaUsun()
        {
            int id_osoba = Convert.ToInt32(HttpContext.Request.Query["id_osoba"]);
            int id_zwierzak = Convert.ToInt32(HttpContext.Request.Query["id_zwierzak"]);

            var cookieUserId = Request.Cookies["CookieUserID"];
            if (string.IsNullOrEmpty(cookieUserId))
            {
                return RedirectToAction("Index", "Login");
            }

            var zwierzak = await _context.zwierzak
                .Include(z => z.Rezerwacja)
                .Where(z => z.id_zwierzak == id_zwierzak && z.Rezerwacja != null)
                .FirstOrDefaultAsync();

            if (zwierzak == null)
            {
                return RedirectToAction("Rezerwacja", "Uzytkownik");
            }

            var rezerwacja = await _context.rezerwuj
                .FirstOrDefaultAsync(r => r.id_osoba == id_osoba && r.id_zwierzak == id_zwierzak);

            _context.rezerwuj.Remove(rezerwacja);
            await _context.SaveChangesAsync();

            return RedirectToAction("Rezerwacja", "Uzytkownik");
        }

        public async Task<ActionResult> Adoptuj()
        {
            int id_zwierzak = Convert.ToInt32(HttpContext.Request.Query["id_zwierzak"]);

            var cookieUserId = Request.Cookies["CookieUserID"];
            if (string.IsNullOrEmpty(cookieUserId))
            {
                return RedirectToAction("Index", "Login");
            }

            var rezerwacja = await _context.rezerwuj
                .FirstOrDefaultAsync(r => r.id_zwierzak == id_zwierzak);

            if(rezerwacja != null)
            {
                _context.rezerwuj.Remove(rezerwacja);
                await _context.SaveChangesAsync();
            }

            var zwierzak = await _context.zwierzak
                .Where(z => z.id_zwierzak == id_zwierzak)
                .FirstOrDefaultAsync();

            zwierzak.czy_adoptowany = true;
            _context.zwierzak.Update(zwierzak);
            await _context.SaveChangesAsync();

            return RedirectToAction("Zwierzaki", "Uzytkownik");
        }

    }
}
