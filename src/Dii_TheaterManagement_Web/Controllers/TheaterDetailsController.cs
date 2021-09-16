using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dii_TheaterManagement_Web.Clients;
using Dii_TheaterManagement_Web.Models;
using Dii_TheaterManagement_Web.Features.FakeUsers;

namespace Dii_TheaterManagement_Web.Controllers
{
    public class TheaterDetailsController : Controller
    {
        private readonly TheaterManagementBffClient _bffClient;
        private readonly FakeUserManager _fakeUserManager;

        public TheaterDetailsController(TheaterManagementBffClient bffClient, FakeUserManager fakeUserManager)
        {
            _bffClient = bffClient;
            _fakeUserManager = fakeUserManager;
        }

        // GET: TheaterDetails
        public async Task<IActionResult> Index()
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode))
            {
                return View("NoAssignedTheater");
            }

            var detailedTheater = await _bffClient.ApiTheaterdetailsGetAsync();
            if (detailedTheater == null)
            {
                return NotFound();
            }

            var vm = new TheaterDetailsViewModel(detailedTheater);
            return View("Details", vm);
        }

        public async Task<IActionResult> Edit()
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode))
            {
                return View("NoAssignedTheater");
            }

            var detailedTheater = await _bffClient.ApiTheaterdetailsGetAsync();
            if (detailedTheater == null)
            {
                return NotFound();
            }

            var themes = await _bffClient.ApiThemesAsync();
            List<SelectListItem> themeNames = 
                themes.Select(
                    t => new SelectListItem { Value = t.Name, Text = t.Name, Selected = t.Name == detailedTheater.WebSiteTheme.Name}).ToList();
            ViewBag.ThemeNames = themeNames;
            var vm = new TheaterDetailsViewModel(detailedTheater);
            return View(vm);
        }

        // POST: TheaterDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("TheaterName,AddressLine1,AddressLine2,City,State,Zip,ThemeName")] TheaterDetailsViewModel vm)
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode))
            {
                return View("NoAssignedTheater");
            }
            DetailedTheater detailedTheater = vm.AsDetailedTheater();
            await _bffClient.ApiTheaterdetailsPutAsync(detailedTheater);
            return RedirectToAction(nameof(Index));
        }
    }
}
