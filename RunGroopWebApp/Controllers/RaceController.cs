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
        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAysnc(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {

                Title = race.Title,
                Description = race.Description,
                URL = race.Image,
                AddressId = (int)race.AddressId,
                Address = race.Address,
                RaceCategory = race.RaceCategory
            };

            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Error", raceVM);
            }
            var userRace = await _raceRepository.GetByIdAysncNoTracking(id);
            if (userRace != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);
                var race = new Race
                {
                    Id = id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = (int)raceVM.AddressId,
                    Address = raceVM.Address,
                    RaceCategory = raceVM.RaceCategory
                };
                _raceRepository.Update(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Club not found");
                return View("Error", raceVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAysnc(id);
            if (raceDetails == null) return View("Error");
            return View(raceDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAysnc(id);
            if (raceDetails == null) return View("Error");
            try
            {
                await _photoService.DeletePhotoAsync(raceDetails.Image);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete photo");
                return View(raceDetails);
            }
            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }

    }
}
