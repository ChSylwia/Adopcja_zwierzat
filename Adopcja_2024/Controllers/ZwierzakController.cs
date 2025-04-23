using Adopcja_2024.Data;
using Adopcja_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Adopcja_2024.Controllers
{
    public class ZwierzakController : Controller
    {
        private readonly MyDbContext _context;
        List<Zwierzak> zwierzak = new List<Zwierzak>();
        ZwierzakDodawanie dodawanie_select = new ZwierzakDodawanie();
        public ZwierzakController(MyDbContext context)
        {
            _context = context;
        }

        public List<Zwierzak> GetZwierzak()
        {
            var zwierzaki = _context.zwierzak
                .Include(z => z.Rasa)
                .Include(z => z.Wiek)
                .Include(z => z.Wielkosc)
                .Where(z => z.czy_adoptowany == false) 
                .Select(z => new Zwierzak
                {
                    id_zwierzak = z.id_zwierzak,
                    imie = z.imie,
                    czy_samiczka = z.czy_samiczka,
                    id_wiek = z.id_wiek,
                    id_wielkosc = z.id_wielkosc,
                    id_rasa = z.id_rasa,   
                    opis = z.opis,
                    id_osoby = z.id_osoby,
                    zdjecie = z.zdjecie
                })
                .ToList();
            return zwierzaki;
        }
        public Zwierzak GetZwierzakSzczegol(int id)
        {
            var zwierzak = _context.zwierzak
                .Include(z => z.Rasa)
                .ThenInclude(r => r.Gatunek)
                .Include(z => z.Wiek)
                .Include(z => z.Wielkosc)
                .Where(z => z.id_zwierzak == id)
                .FirstOrDefault(z => z.id_zwierzak == id);

            return zwierzak;
        }
        public List<Gatunek> GetGatunek()
        {
            var gatunki = _context.gatunek.ToList(); 

            return gatunki;
        }
        public List<Wiek> GetWiek()
        {
            var wieki = _context.wiek.ToList();  

            return wieki;
        }
        public List<Wielkosc> GetWielkosc()
        {
            var wielkosci = _context.wielkosc.ToList();  

            return wielkosci;
        }

        public List<Rasa> GetRasa()
        {
            var rasy = _context.rasa.ToList();
            return rasy;
        }

        public List<Rezerwacja> GetRezerwacja(int id_zwierzak)
        {
            return _context.rezerwuj
                .Where(r => r.id_zwierzak == id_zwierzak)
                .Include(r => r.Osoba) 
                .ToList();  
        }

        public string AddZwierzak(Zwierzak z, IFormFile? image)
        {
            try
            {
                byte[] imageData = null;

                if (image != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        imageData = memoryStream.ToArray();
                    }
                }

                // Tworzymy nowy obiekt zwierzaka
                var newZwierzak = new Zwierzak
                {
                    imie = z.imie,
                    czy_samiczka = z.czy_samiczka,
                    id_wiek = z.id_wiek,
                    id_wielkosc = z.id_wielkosc,
                    id_rasa = z.id_rasa,
                    opis = z.opis,
                    id_osoby = z.id_osoby,
                    zdjecie = imageData,
                    czy_adoptowany = false 
                };

                _context.zwierzak.Add(newZwierzak);
                _context.SaveChanges();

                return "Success";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return "Error";
            }
        }

        public ActionResult Index(bool[] plec, int[] wiek, int[] gatunek)
        {
            var zwierzaki = _context.zwierzak
                .Where(z => z.Rezerwacja == null && z.czy_adoptowany == false)
                .Include(z => z.Wiek)
                .Include(z => z.Rasa)
                    .ThenInclude(r => r.Gatunek)
                .Include(z => z.Wielkosc)
                .OrderByDescending(z => z.id_zwierzak)
                .AsQueryable();

            if (plec != null && plec.Any())
            {
                zwierzaki = zwierzaki.Where(z => plec.Contains(z.czy_samiczka));
            }

            if (wiek != null && wiek.Any())
            {
                zwierzaki = zwierzaki.Where(z => wiek.Contains(z.id_wiek));
            }

            if (gatunek != null && gatunek.Any())
            {
                zwierzaki = zwierzaki.Where(z => gatunek.Contains(z.Rasa.id_gatunek));
            }

            dodawanie_select.zwierzaki = zwierzaki.ToList();
            dodawanie_select.rasa = _context.rasa.ToList();
            dodawanie_select.gatunek = _context.gatunek.ToList();
            dodawanie_select.wiek = _context.wiek.ToList();
            dodawanie_select.wielkosc = _context.wielkosc.ToList();

            return View(dodawanie_select);
        }
        public IActionResult Details(int id)
        {
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            var rezerwacja_zwierzak = new ZwierzakRezerwacja
            {
                Zwierzak = GetZwierzakSzczegol(id),

                Rezerwacja = GetRezerwacja(id)
            };

            if (rezerwacja_zwierzak.Zwierzak == null)
            {
                return NotFound();
            }

            return View(rezerwacja_zwierzak);
        
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new ZwierzakDodawanie
            {
                gatunek = _context.gatunek.Include(g => g.Rasa).ToList(),
                rasa = _context.rasa.ToList(),
                wiek = _context.wiek.ToList(),
                wielkosc = _context.wielkosc.ToList(),
                zwierzaki = new List<Zwierzak> { new Zwierzak() } 
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Zwierzak zwierzak, IFormFile? zdjecie)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    var fieldName = error.Key;
                    var errorMessages = error.Value.Errors.Select(e => e.ErrorMessage).ToList();

                    foreach (var message in errorMessages)
                    {
                        Debug.WriteLine($"  Błąd: {message}");
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                var model = new ZwierzakDodawanie
                {
                    gatunek = _context.gatunek.Include(g => g.Rasa).ToList(),
                    rasa = _context.rasa.ToList(),
                    wiek = _context.wiek.ToList(),
                    wielkosc = _context.wielkosc.ToList(),
                    zwierzaki = new List<Zwierzak> { zwierzak }
                };
                return View(model);
            }

            if (Request.Cookies["CookieUserID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            zwierzak.id_osoby = Convert.ToInt32(Request.Cookies["CookieUserID"]);
            zwierzak.czy_adoptowany = false;

            if (zdjecie != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await zdjecie.CopyToAsync(memoryStream);
                    zwierzak.zdjecie = memoryStream.ToArray();
                }
            }

            _context.zwierzak.Add(zwierzak);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Zwierzak");
        }

        // GET: Zwierzak/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IEnumerable<Gatunek> gatunek = _context.gatunek.Include(g => g.Rasa).ToList();
            IEnumerable< Wiek > wiek = _context.wiek.ToList();
            IEnumerable<Wielkosc> wielkosc = _context.wielkosc.ToList();
            Zwierzak zwierzak = _context.zwierzak
                    .Where(z => z.id_zwierzak == id)
                    .Include(z => z.Wiek)
                    .Include(z => z.Rasa)
                        .ThenInclude(r => r.Gatunek)
                    .Include(z => z.Wielkosc)
                    .FirstOrDefault();

            ZwierzakEdit zwierzakEdit = new ZwierzakEdit
            {
                Gatunek = gatunek,
                Wiek = wiek,
                Wielkosc = wielkosc,
                Zwierzak = zwierzak

            };
            if (zwierzakEdit.Zwierzak == null)
            {
                return NotFound();
            }
            return View(zwierzakEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Zwierzak zwierzak, IFormFile? zdjecie)
        {
            zwierzak.id_osoby = Convert.ToInt32(Request.Cookies["CookieUserID"]);
            zwierzak.czy_adoptowany = false;
            if (ModelState.IsValid)
            {
                Zwierzak z = _context.zwierzak.FirstOrDefault(x => x.id_zwierzak == zwierzak.id_zwierzak);

                if (z != null)
                {
                    z.id_wielkosc = zwierzak.id_wielkosc;
                    z.id_wiek = zwierzak.id_wiek;
                    z.id_rasa = zwierzak.id_rasa;
                    z.czy_samiczka = zwierzak.czy_samiczka;
                    z.imie = zwierzak.imie;
                    z.opis = zwierzak.opis;

                    if (zdjecie != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await zdjecie.CopyToAsync(memoryStream);
                            z.zdjecie = memoryStream.ToArray();
                        }
                    }

                    _context.SaveChanges();

                    return RedirectToAction("Zwierzaki", "Uzytkownik");
                }
            }

            return RedirectToAction("Edit");
        }

        [HttpPost, HttpGet]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var cookieUserId = Request.Cookies["CookieUserID"];
                if (string.IsNullOrEmpty(cookieUserId))
                {
                    return RedirectToAction("Index", "Login"); // Brak zalogowanego użytkownika
                }

                var zwierzak = await _context.zwierzak.FindAsync(id);
                if (zwierzak == null)
                {
                    return View();
                }

                if (zwierzak.id_osoby != Convert.ToInt32(Request.Cookies["CookieUserID"]))
                {
                    return View(); // Użytkownik nie ma dostępu
                }

                _context.zwierzak.Remove(zwierzak);
                await _context.SaveChangesAsync();

                return RedirectToAction("Zwierzaki", "Uzytkownik");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Rezerwacja()
        {
            int id_osoba = Convert.ToInt32(HttpContext.Request.Query["id_osoba"]);
            int id_zwierzak = Convert.ToInt32(HttpContext.Request.Query["id_zwierzak"]);

            var cookieUserId = Request.Cookies["CookieUserID"];
            if (string.IsNullOrEmpty(cookieUserId))
            {
                return RedirectToAction("Index", "Login");
            }

            var zwierzak = await _context.zwierzak
                .Where(z => z.id_osoby != Convert.ToInt32(Request.Cookies["CookieUserID"]) && z.id_zwierzak == id_zwierzak && z.czy_adoptowany == false)
                .FirstOrDefaultAsync();

            if (zwierzak == null)
            {
                TempData["ErrorMessage"] = "Nie można zarezerwować tego zwierzaka.";
                return RedirectToAction("Details", "Zwierzak", new { id = id_zwierzak }); ;
            }

            Rezerwacja rezerwacja = new Rezerwacja
            {
                id_osoba = id_osoba,
                id_zwierzak = id_zwierzak
            };

            _context.rezerwuj.Add(rezerwacja);
            await _context.SaveChangesAsync();
            return RedirectToAction("Rezerwacja", "Uzytkownik");

        }

    }
}
