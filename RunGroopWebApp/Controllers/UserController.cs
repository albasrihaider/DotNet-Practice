using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Milage = user.Milage
                };
                result.Add(userViewModel);
            }
            return View(result);
        }


        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Milage = user.Milage
            };

            return View(userDetailViewModel);
        }
    }
}
