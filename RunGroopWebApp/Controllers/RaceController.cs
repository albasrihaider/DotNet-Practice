using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;


namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;

        [ActivatorUtilitiesConstructor]
        public RaceController(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var race = await _raceRepository.GetByIdAysnc(id);

            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }
    }
}
