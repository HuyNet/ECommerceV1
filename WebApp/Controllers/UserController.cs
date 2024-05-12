using Microsoft.AspNetCore.Mvc;
using ViewModels.System.Users;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginRequest request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return View(ModelState);
        //    }

        //    var token = await _userApiClient.Authenicate(request);

        //    return View(token);
        //}
    }
}
