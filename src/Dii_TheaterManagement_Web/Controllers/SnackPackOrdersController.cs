using DiiCommon;
using DiiCommon.Time;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dii_TheaterManagement_Web.Clients;
using Dii_TheaterManagement_Web.Features.FakeUsers;
using Dii_TheaterManagement_Web.Models;

namespace Dii_TheaterManagement_Web.Controllers
{
    public class SnackPackOrdersController : Controller
    {
        private readonly TheaterManagementBffClient _bffClient;
        private readonly FakeUserManager _fakeUserManager;

        public SnackPackOrdersController(TheaterManagementBffClient bffClient, FakeUserManager fakeUserManager)
        {
            _bffClient = bffClient;
            _fakeUserManager = fakeUserManager;
        }

        // GET: SnackPackOrders
        public async Task<IActionResult> Index()
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string _)) { return View("NoAssignedTheater"); }

            var snackPackOrders = await _bffClient.ApiSnackpackordersGetAsync();

            var currentMonth = new Month(DateTime.Now);
            var monthsStartingAt = new MonthsStartingAt(currentMonth, DiiConstants.Theaters.BookingWindowSizeInMonths);

            // For the current month and the standard number of upcoming months, show the user the list of monthly snack orders.
            List<MonthlySnackPackOrderViewModel> vms = monthsStartingAt.Values
                .Select(m => new MonthlySnackPackOrderViewModel(m, snackPackOrders.SingleOrDefault(b => b.MonthId == m.MonthId)))
                .ToList();
            return View(vms);
        }

        // GET: SnackPackOrders/Details/5
        public async Task<IActionResult> Details(int? monthId)
        {
            if (monthId == null)
            {
                return NotFound();
            }

            if (!_fakeUserManager.TryGetTheaterInfo(out string _)) { return View("NoAssignedTheater"); }

            var snackPackOrder = await _bffClient.ApiSnackpackordersGetAsync(monthId.Value);

            var vm = new MonthlySnackPackOrderViewModel(monthId.Value, snackPackOrder);
            return View(vm);
        }

        // GET: SnackPackOrders/Create
        public IActionResult Create(int monthId)
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string _)) { return View("NoAssignedTheater"); }

            var vm = new MonthlySnackPackOrderViewModel(monthId, null);
            return View(vm);
        }

        // POST: SnackPackOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonthId,OrderCount")] MonthlySnackPackOrderViewModel vm)
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            if (ModelState.IsValid)
            {
                SnackPackOrder snackPackOrder = vm.AsSnackPackOrder(theaterCode);
                await _bffClient.ApiSnackpackordersPutAsync(vm.MonthId, snackPackOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: SnackPackOrders/Edit/5
        public async Task<IActionResult> Edit(int? monthId)
        {
            if (monthId == null) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var snackPackOrder = await _bffClient.ApiSnackpackordersGetAsync(monthId.Value);
            if (snackPackOrder == null)
            {
                return NotFound();
            }

            var vm = new MonthlySnackPackOrderViewModel(monthId.Value, snackPackOrder);
            return View(vm);
        }

        // POST: SnackPackOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int monthId, [Bind("MonthId,OrderCount")] MonthlySnackPackOrderViewModel vm)
        {
            if (monthId != vm.MonthId) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            if (ModelState.IsValid)
            {
                var snackPackOrder = vm.AsSnackPackOrder(theaterCode);
                await _bffClient.ApiSnackpackordersPutAsync(vm.MonthId, snackPackOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: SnackPackOrders/Delete/5
        public async Task<IActionResult> Delete(int? monthId)
        {
            if (monthId == null || monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var snackPackOrder = await _bffClient.ApiSnackpackordersGetAsync(monthId.Value);
            if (snackPackOrder == null)
            {
                return NotFound();
            }

            var vm = new MonthlySnackPackOrderViewModel(monthId.Value, snackPackOrder);
            return View(vm);
        }

        // POST: SnackPackOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int monthId)
        {
            if (monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            await _bffClient.ApiSnackpackordersDeleteAsync(monthId);
            return RedirectToAction(nameof(Index));
        }
    }
}
