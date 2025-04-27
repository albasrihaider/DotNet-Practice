using Microsoft.AspNetCore.Mvc;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
