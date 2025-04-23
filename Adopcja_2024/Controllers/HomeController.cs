using System.Diagnostics;
using Adopcja_2024.Data;
using Adopcja_2024.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Adopcja_2024.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        private List<Zwierzak> GetZwierzaki()
        {
            return _context.zwierzak
                .Where(z => !z.czy_adoptowany && z.Rezerwacja == null)
                .OrderByDescending(z => z.id_zwierzak)
                .Take(6)
                .Include(z => z.Wiek)     
                .Include(z => z.Wielkosc)
                .Include(z => z.Rasa)
                .ToList();
        }

        private List<Ogloszenie> GetOgloszenia()
        {
            return _context.ogloszenie
                .OrderByDescending(o => o.id_ogloszenie)
                .Take(4)
                .ToList();
        }
        public IActionResult Index()
        {
            var homeData = new ZwierzakOgloszenia
            {
                zwierzaki = GetZwierzaki(),
                ogloszenia = GetOgloszenia()
            };
            return View(homeData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
