using Microsoft.AspNetCore.Mvc;

namespace Dii_TheaterManagement_Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return SignOut(AppConstants.DefaultAuthenticationScheme, AppConstants.OidcAuthenticationScheme);
        }
    }
}
