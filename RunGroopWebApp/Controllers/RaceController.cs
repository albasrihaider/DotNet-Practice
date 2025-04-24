using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.ViewModels;


namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        [ActivatorUtilitiesConstructor]
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceMV)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceMV.Image);
                var race = new Race
                {
                    Title = raceMV.Title,
                    Description = raceMV.Description,
                    Image = result.Url.ToString(),
                    Address = new Address()
                    {
                        City = raceMV.Address.City,
                        State = raceMV.Address.State,
                        Street = raceMV.Address.Street,

                    },

                    RaceCategory = raceMV.RaceCategory
                };

                _raceRepository.Add(race);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceMV);

        } 

    }
}
