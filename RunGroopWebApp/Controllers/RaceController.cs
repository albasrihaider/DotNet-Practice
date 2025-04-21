using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;


namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly ApplicationDbContext _context;

        [ActivatorUtilitiesConstructor]
        public RaceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Race> races= _context.Races.ToList();
            return View(races);
        }

        public IActionResult Detail(int id)
        {
            var race = _context.Races
      .Include(c => c.Address) // Eagerly load the Address navigation property
      .FirstOrDefault(c => c.Id == id);

            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }
    }
}
