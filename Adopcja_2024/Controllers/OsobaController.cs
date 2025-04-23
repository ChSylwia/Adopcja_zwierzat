using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Adopcja_2024.Data;
using Adopcja_2024.Models;
using System.Security.Cryptography;
using System.Text;

namespace Adopcja_2024.Controllers
{
    public class OsobaController : Controller
    {
        private readonly MyDbContext _context;

        public OsobaController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Osoba
        public async Task<IActionResult> Index(int id)
        {
            /*return View(await _context.osoba
                .Include(o => o.Miasto)
                .ToListAsync());*/

            if (id != Convert.ToInt32(Request.Cookies["CookieUserID"]))
            {
                return RedirectToAction("Index", "Osoba", new { id = Convert.ToInt32(Request.Cookies["CookieUserID"]) });
            }
            Osoba osoba = await _context.osoba
                .Include(o => o.Miasto)
                .Include(o => o.Instytucja)
                    .ThenInclude(i => i.Miasto)
                .FirstOrDefaultAsync(o => o.id_osoba == id);
            return View(osoba);
        }

        // GET: Osoba/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var osoba = await _context.osoba
                .FirstOrDefaultAsync(m => m.id_osoba == id);
            if (osoba == null)
            {
                return NotFound();
            }

            return View(osoba);
        }

        // GET: Osoba/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Osoba/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_osoba,imie,nazwisko,czy_kobieta,nazwa_miasta, nr_telefonu,id_instytucja,email,haslo, czy_instytucja, nazwa_instytucji, miasto, ulica, kod_pocztowy, nr_domu, nr_lokalu, nr_telefonu_instytucja, email_instytucja")] OsobaRejestracja osoba)
        {
            if (string.IsNullOrEmpty(osoba.haslo))
            {
                ModelState.AddModelError("haslo", "Hasło jest wymagane.");
            }
            if (ModelState.IsValid)
            {
                Miasto miasto = null;
                if (!string.IsNullOrWhiteSpace(osoba.nazwa_miasta))
                {
                    miasto = await _context.miasto
                        .FirstOrDefaultAsync(m => m.nazwa.ToLower() == osoba.nazwa_miasta.ToLower());

                    if (miasto == null)
                    {
                        // Tworzenie nowego miasta, jeśli nie istnieje
                        miasto = new Miasto { nazwa = osoba.nazwa_miasta };
                        _context.miasto.Add(miasto);
                        await _context.SaveChangesAsync();
                    }
                }

                MD5 md5 = MD5.Create();
                var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(osoba.haslo));
                string hash_password = Convert.ToHexString(byte_data);

                Miasto miasto_instytucja = null;
                Instytucja instytucja = null;
                if (osoba.czy_instytucja == true)
                {
                    if (!string.IsNullOrWhiteSpace(osoba.miasto))
                    {
                        miasto_instytucja = await _context.miasto
                            .FirstOrDefaultAsync(m => m.nazwa.ToLower() == osoba.miasto.ToLower());

                        if (miasto_instytucja == null)
                        {
                            // Tworzenie nowego miasta, jeśli nie istnieje
                            miasto_instytucja = new Miasto { nazwa = osoba.nazwa_miasta };
                            _context.miasto.Add(miasto_instytucja);
                            await _context.SaveChangesAsync();
                        }
                    }
                    instytucja = new Instytucja
                    {
                        nazwa = osoba.nazwa_instytucji,
                        ulica = osoba.ulica,
                        nr_domu = osoba.nr_domu,
                        nr_lokalu = osoba.nr_lokalu,
                        kod_pocztowy = osoba.kod_pocztowy,
                        id_miasto = miasto_instytucja.id_miasto,
                        nr_telefonu = osoba.nr_telefonu_instytucja,
                        e_mail = osoba.email_instytucja,
                    };
                    /*stytucja.nazwa = osoba.nazwa_instytucji;
                    instytucja.ulica = osoba.ulica;
                    instytucja.nr_domu = osoba.nr_domu;
                    instytucja.nr_lokalu = osoba.nr_lokalu;
                    instytucja.kod_pocztowy = osoba.kod_pocztowy;
                    instytucja.id_miasto = miasto_instytucja.id_miasto;
                    instytucja.nr_telefonu = osoba.nr_telefonu_instytucja;
                    instytucja.e_mail = osoba.email_instytucja;*/

                    _context.instytucja.Add(instytucja);
                    await _context.SaveChangesAsync();
                }

                Osoba nowyUzytkownik  = new Osoba()
                {
                    imie = osoba.imie,
                    nazwisko = osoba.nazwisko,
                    czy_kobieta = osoba.czy_kobieta,
                    id_miasto = miasto.id_miasto,
                    nr_telefonu = osoba.nr_telefonu,
                    email = osoba.email,
                    haslo = hash_password,
                };

                if(instytucja != null)
                {
                    nowyUzytkownik.id_instytucja = instytucja.id_instytucja;
                }

                _context.Add(nowyUzytkownik);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Super! Twoje konto zostało poprawnie utworzone! Zaloguj się na nie.";
                return RedirectToAction("Index", "Login");
            }
            return View(osoba);
        }

        // GET: Osoba/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var osoba = await _context.osoba
                .Include(o => o.Miasto)
                .Include(o => o.Instytucja)
                    .ThenInclude(i => i.Miasto)
                .FirstOrDefaultAsync(m => m.id_osoba == id);
            if (osoba == null)
            {
                return NotFound();
            }
            return View(osoba);
        }

        // POST: Osoba/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Osoba osoba)
        {
            if (id != osoba.id_osoba)
            {
                return NotFound();
            }

            //ModelState.Remove("haslo");
            if (ModelState.IsValid)
            {
                Osoba o = _context.osoba.Include(o => o.Instytucja).FirstOrDefault(o => o.id_osoba == id);
                try
                {
                    Miasto miasto = _context.miasto.Where(m => m.nazwa == osoba.Miasto.nazwa).FirstOrDefault();
                    Miasto miasto_instytucja = null;
                    if (osoba.Instytucja?.Miasto?.nazwa != null)
                    {
                        miasto_instytucja = _context.miasto
                            .Where(m => m.nazwa == osoba.Instytucja.Miasto.nazwa)
                            .FirstOrDefault();
                    }
                    if(miasto == null)
                    {
                        miasto = new Miasto { nazwa = osoba.Miasto.nazwa };
                        _context.miasto.Add(miasto);
                        await _context.SaveChangesAsync();
                        o.Miasto = miasto;
                    }
                    if (miasto.id_miasto != osoba.Miasto.id_miasto)
                    {
                        o.Miasto = miasto;
                    }
                    if(miasto_instytucja != null)
                    {
                        if (miasto_instytucja.id_miasto != osoba.Instytucja.Miasto.id_miasto)
                        {
                            o.Instytucja.Miasto = miasto_instytucja;
                        }
                    }
                    if(osoba.Instytucja?.Miasto?.nazwa != null)
                    {
                        miasto_instytucja = new Miasto { nazwa = osoba.Instytucja.Miasto.nazwa };
                        _context.miasto.Add(miasto_instytucja);
                        await _context.SaveChangesAsync();
                        o.Instytucja.Miasto = miasto_instytucja;
                    }              

                    if(!string.IsNullOrEmpty(osoba.haslo))
                    {
                        MD5 md5 = MD5.Create();
                        var byte_data = md5.ComputeHash(Encoding.UTF8.GetBytes(osoba.haslo));
                        string hash_password = Convert.ToHexString(byte_data);
                        o.haslo = hash_password;
                    }

                    o.imie = osoba.imie;
                    o.nazwisko = osoba.nazwisko;
                    o.email = osoba.email;
                    o.czy_kobieta = osoba.czy_kobieta;
                    o.nr_telefonu = osoba.nr_telefonu;
                    if (o.Instytucja != null)
                    {
                        o.Instytucja.nazwa = osoba.Instytucja.nazwa;
                        o.Instytucja.e_mail = osoba.Instytucja.e_mail;
                        o.Instytucja.nr_telefonu = osoba.Instytucja.nr_telefonu;
                        o.Instytucja.ulica = osoba.Instytucja.ulica;
                        o.Instytucja.nr_domu = osoba.Instytucja.nr_domu;
                        o.Instytucja.nr_lokalu = osoba.Instytucja.nr_lokalu;
                        o.Instytucja.Miasto = miasto_instytucja;
                    }

                    
                    _context.Update(o);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OsobaExists(osoba.id_osoba))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(osoba);
        }

        // GET: Osoba/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var osoba = await _context.osoba
                .FirstOrDefaultAsync(m => m.id_osoba == id);
            if (osoba == null)
            {
                return NotFound();
            }

            return View(osoba);
        }

        // POST: Osoba/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var osoba = await _context.osoba.FindAsync(id);
            if (osoba != null)
            {
                _context.osoba.Remove(osoba);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OsobaExists(int id)
        {
            return _context.osoba.Any(e => e.id_osoba == id);
        }
    }
}
