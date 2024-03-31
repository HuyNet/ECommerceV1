using Microsoft.AspNetCore.Mvc;

namespace WebApp.Admin.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
