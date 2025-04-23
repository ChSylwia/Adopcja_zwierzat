using Adopcja_2024.Data;
using Adopcja_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Adopcja_2024.Controllers
{
    public class OgloszeniaController : Controller
    {
        private readonly MyDbContext _context;
        List<Ogloszenie> ogloszenia = new List<Ogloszenie>();
        public OgloszeniaController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ogloszenie>> GetOgloszenia()
        {
            
             ogloszenia = await _context.ogloszenie
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
            
            return ogloszenia;
        }


        public async Task<Ogloszenie> GetOgloszeniaSzczegol(int id)
        {
             var ogloszenia = await _context.ogloszenie
                .Where(o => o.id_ogloszenie == id)
                .Select(o => new Ogloszenie
                {
                    id_ogloszenie = o.id_ogloszenie,
                    tytul = o.tytul,
                    data = o.data,
                    opis = o.opis,
                    zdjecie = o.zdjecie,
                    Osoba = new Osoba
                    {
                        id_osoba = o.Osoba.id_osoba,
                        imie = o.Osoba.imie,
                        nazwisko = o.Osoba.nazwisko,
                        email = o.Osoba.email,
                        nr_telefonu = o.Osoba.nr_telefonu,
                        Instytucja = o.Osoba.Instytucja != null
                    ? new Instytucja
                    {
                        id_instytucja = o.Osoba.Instytucja.id_instytucja,
                        nazwa = o.Osoba.Instytucja.nazwa
                    }
                    : null
                    }
                })
                .FirstOrDefaultAsync();
            return ogloszenia;
        }
        private async Task<string> AddOgloszenie(Ogloszenie o, IFormFile? image)
        {
            try
            {
                if (image != null)
                {
                    using var memoryStream = new MemoryStream();
                    await image.CopyToAsync(memoryStream);
                    o.zdjecie = memoryStream.ToArray();
                }

                _context.ogloszenie.Add(o);
                await _context.SaveChangesAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException?.Message);
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }
        }
        [HttpGet]
        public ActionResult Create()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_ogloszenie,tytul,data,opis,zdjecie,id_osoba")] Ogloszenie o, IFormFile? zdjecie)
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
            if (ModelState.IsValid)
            {
                o.id_osoba = Convert.ToInt32(Request.Cookies["CookieUserID"]);
                string result = await AddOgloszenie(o, zdjecie);

                if (result == "Success")
                {
                    return RedirectToAction("Index", "Ogloszenia");
                }
                else
                {
                    ModelState.AddModelError("", "Wystąpił błąd przy dodawaniu ogłoszenia.");
                    return View(o);
                }
            }

            return RedirectToAction("Create");
        }



        public async Task<IActionResult> DeleteOgloszenie(int id)
        {
            var userId = HttpContext.Request.Cookies["CookieUserID"];

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var ogloszenie = await _context.ogloszenie
                .FirstOrDefaultAsync(o => o.id_ogloszenie == id && o.id_osoba.ToString() == userId);

            if (ogloszenie == null)
            {
                return NotFound("Ogłoszenie nie zostało znalezione lub użytkownik nie ma do niego dostępu.");
            }

            _context.ogloszenie.Remove(ogloszenie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: OgloszeniaController/Index
        public async Task<ActionResult> Index()
        {
          
            var ogloszenia = await GetOgloszenia();
            Debug.WriteLine(ogloszenia);

            return View(ogloszenia); 
        }

        // GET: OgloszeniaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var ogloszenie = await GetOgloszeniaSzczegol(id);

            if (ogloszenie == null)
            {
                return NotFound();
            }

            return View(ogloszenie);
        }

        // GET: Osoba/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ogloszenie = await _context.ogloszenie.FindAsync(id);
            if (ogloszenie == null)
            {
                return NotFound();
            }
            return View(ogloszenie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Ogloszenie ogloszenie, IFormFile? zdjecie)
        {
            ogloszenie.id_osoba = Convert.ToInt32(Request.Cookies["CookieUserID"]);
            if (ModelState.IsValid)
            {
                Ogloszenie o = _context.ogloszenie.Find(ogloszenie.id_ogloszenie);

                if (o != null)
                {
                    o.tytul = ogloszenie.tytul;
                    o.opis = ogloszenie.opis;
                    o.data = ogloszenie.data;
                    if (zdjecie != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await zdjecie.CopyToAsync(memoryStream);
                            o.zdjecie = memoryStream.ToArray();
                        }
                    }
                    _context.SaveChanges();

                    return RedirectToAction("Ogloszenia", "Uzytkownik");  // Przekierowanie po udanej aktualizacji
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
                    return View(); // Brak zalogowanego użytkownika
                }

                var ogloszenie = await _context.ogloszenie.FindAsync(id);
                if (ogloszenie == null)
                {
                    return View();
                }

                if (ogloszenie.id_osoba != Convert.ToInt32(Request.Cookies["CookieUserID"]))
                {
                    return View(); // Użytkownik nie ma dostępu
                }

                _context.ogloszenie.Remove(ogloszenie);
                await _context.SaveChangesAsync();

                return RedirectToAction("Ogloszenia", "Uzytkownik");
            }
            catch
            {
                return View();
            }
        }
    }
}
