using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task< IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAysnc(id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        public IActionResult Create()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            CreateClubViewModel createClubViewModel = new CreateClubViewModel() {AppUserId = curUser };
            return View(createClubViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {

              var result= await _photoService.AddPhotoAsync(clubVM.Image);
              var club = new Club
              {
                  Title = clubVM.Title,
                  Description = clubVM.Description,
                  Image = result.Url.ToString(),
                  AppUserId = clubVM.AppUserId, 
                  Address = new Address()
                  {
                      City = clubVM.Address.City,
                      State = clubVM.Address.State,
                      Street = clubVM.Address.Street,
                     
                  },

                  ClubCategory = clubVM.ClubCategory
              };    

            _clubRepository.Add(club);
            return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }
    public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAysnc(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
              
                Title = club.Title,
                Description = club.Description,
                URL = club.Image,
                AddressId = (int)club.AddressId,
                Address = club.Address,
                ClubCategory = club.ClubCategory
            };

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Error", clubVM);
            }
            var userClub = await _clubRepository.GetByIdAysncNoTracking(id);
            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = (int)clubVM.AddressId,
                    Address = clubVM.Address,
                    ClubCategory = clubVM.ClubCategory
                };
                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Club not found");
                return View("Error", clubVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAysnc(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAysnc(id);
            if (clubDetails == null) return View("Error");
            try
            {
                await _photoService.DeletePhotoAsync(clubDetails.Image);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Could not delete photo");
                return View(clubDetails);
            }
            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }

    }
}
