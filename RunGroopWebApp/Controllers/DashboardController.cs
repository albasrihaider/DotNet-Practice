using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _httpContextAccessor = httpContextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit (EditUserDashboardViewModel editVM, AppUser user ,ImageUploadResult photoResult)
        {
            user.UserName = editVM.UserName;
            user.Pace = editVM.Pace;
            user.Milage = editVM.Milage;
            user.City = editVM.City;
            user.State = editVM.State;
            user.ProfileImageUrl = photoResult.Url.ToString();
        }
        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(curUserId);
           if(user == null) return View("Error");
           var editUserDashboardViewModel = new EditUserDashboardViewModel()
           {
               Id = user.Id,
               UserName = user.UserName,
               Pace = user.Pace,
               Milage = user.Milage,
               ProfileImageUrl = user.ProfileImageUrl,
               City = user.City,
               State = user.State,
           };
            return View(editUserDashboardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        { 
            if(!ModelState.IsValid)
            {
              ModelState.AddModelError("", "Failed to edit profile");
              return View("EditUserProfile", editVM);
            }
            var user = await _dashboardRepository.GetUserByIdNoTracking(editVM.Id);
            if(user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var result = await _photoService.AddPhotoAsync(editVM.Image);
                if (result.Error != null)
                {
                    ModelState.AddModelError("", "Failed to upload image");
                    return View("EditUserProfile", editVM);
                }
               MapUserEdit(editVM, user, result);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var result = await _photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(editVM, user, result);
                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            
        }
    }
}
