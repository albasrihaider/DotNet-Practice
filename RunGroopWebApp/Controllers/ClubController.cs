using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ClubController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Club> clubs = _context.Clubs.ToList();
            return View(clubs);
        }

        public IActionResult Detail(int id)
        {
            var club = _context.Clubs
      .Include(c => c.Address) // Eagerly load the Address navigation property
      .FirstOrDefault(c => c.Id == id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }
    }
}
