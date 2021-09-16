using Microsoft.AspNetCore.Mvc;
using Dii_TheaterManagement_Web.Clients;
using Dii_TheaterManagement_Web.Features.FakeUsers;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dii_TheaterManagement_Web.Controllers
{
    public class FakeUsersController : Controller
    {
        private readonly FakeUserManager _fakeUserManager;
        private readonly TheaterManagementBffClient _bffClient;

        public FakeUsersController(FakeUserManager fakeUserManager, TheaterManagementBffClient bffClient)
        {
            _fakeUserManager = fakeUserManager;
            _bffClient = bffClient;
        }

        // GET: FakeUsersController
        public async Task<ActionResult> Index()
        {
            IEnumerable<FakeUser> users = await _bffClient.FakeUsersGetAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Login(string sub)
        {
            var user = await _bffClient.FakeUsersGetAsync(sub);
            _fakeUserManager.SignInUser(user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            _fakeUserManager.SignOutCurrentUser();
            return RedirectToAction(nameof(Index));
        }
    }
}
