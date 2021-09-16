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
    public class CurrentBookingsController : Controller
    {
        private readonly TheaterManagementBffClient _bffClient;
        private readonly FakeUserManager _fakeUserManager;

        public CurrentBookingsController(TheaterManagementBffClient bffClient, FakeUserManager fakeUserManager)
        {
            _bffClient = bffClient;
            _fakeUserManager = fakeUserManager;
        }

        // GET: CurrentBookings
        public async Task<IActionResult> Index()
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var bookings = await _bffClient.ApiBookingsGetAsync();

            var currentMonth = new Month(DateTime.Now);
            var monthsStartingAt = new MonthsStartingAt(currentMonth, DiiConstants.Theaters.BookingWindowSizeInMonths);

            // For the current month and the standard number of upcoming months, show the user the list of monthly Bookings.
            List<TheaterBookingViewModel> vms = monthsStartingAt.Values
                .Select(m => new TheaterBookingViewModel(bookings.SingleOrDefault(b => b.MonthId == m.MonthId), m))
                .ToList();
            return View(vms);
        }

        // GET: CurrentBookings/Details/5
        public async Task<IActionResult> Details(int? monthId)
        {
            if (monthId == null || monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var booking = await _bffClient.ApiBookingsGetAsync(monthId.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var theaterBookingViewModel = new TheaterBookingViewModel(booking, monthId.Value);
            return View(theaterBookingViewModel);
        }

        // TODO: Rename this Action and the View to "Book"

        // GET: CurrentBookings/Create
        public async Task<IActionResult> CreateAsync(int monthId)
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var month = new Month(monthId);
            var theaterBookingViewModel = new TheaterBookingViewModel(null, month);

            ViewBag.Movies = await SelectListOfMoviesAsync();
            return View(theaterBookingViewModel);
        }

        // TODO: Rename this Action and the View to "Book"

        // POST: CurrentBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,MonthId")] TheaterBookingViewModel vm)
        {
            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            if (ModelState.IsValid)
            {
                Booking booking = vm.AsBooking();
                await _bffClient.ApiBookingsPutAsync(vm.MonthId, booking);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Movies = await SelectListOfMoviesAsync(vm.MovieId);
            return View(vm);
        }

        // GET: CurrentBookings/Edit/5
        public async Task<IActionResult> Edit(int? monthId)
        {
            if (monthId == null || monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var booking = await _bffClient.ApiBookingsGetAsync(monthId.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var theaterBookingViewModel = new TheaterBookingViewModel(booking, monthId.Value);
            ViewBag.Movies = await SelectListOfMoviesAsync(booking.MovieId);
            return View(theaterBookingViewModel);
        }

        // POST: CurrentBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int monthId, [Bind("MovieId,MonthId")] TheaterBookingViewModel vm)
        {
            if (monthId != vm.MonthId) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            if (ModelState.IsValid)
            {
                var booking = vm.AsBooking();
                await _bffClient.ApiBookingsPutAsync(vm.MonthId, booking);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Movies = await SelectListOfMoviesAsync(vm.MovieId);
            return View(vm);
        }

        // GET: CurrentBookings/Delete/5
        public async Task<IActionResult> Delete(int? monthId)
        {
            if (monthId == null || monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            var booking = await _bffClient.ApiBookingsGetAsync(monthId.Value);
            if (booking == null)
            {
                return NotFound();
            }

            var theaterBookingViewModel = new TheaterBookingViewModel(booking, monthId.Value);
            return View(theaterBookingViewModel);
        }

        // POST: CurrentBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int monthId)
        {
            if (monthId == 0) { return NotFound(); }

            if (!_fakeUserManager.TryGetTheaterInfo(out string theaterCode)) { return View("NoAssignedTheater"); }

            await _bffClient.ApiBookingsDeleteAsync(monthId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<SelectList> SelectListOfMoviesAsync(long? selectedMovieId = null)
        {
            var movies = await _bffClient.ApiMoviesGetAsync();
            if (selectedMovieId.HasValue)
            {
                List<SelectListItem> movieNames =
                    movies.Select(
                        t => new SelectListItem { Value = t.MovieId.ToString(), Text = t.MovieMetadata.Title, Selected = t.MovieId == selectedMovieId }).ToList();
                return new SelectList(movies, "MovieId", "Title", selectedMovieId);
            }
            else
            {
                List<SelectListItem> movieNames =
                    movies.Select(
                        t => new SelectListItem { Value = t.MovieId.ToString(), Text = t.MovieMetadata.Title }).ToList();
                return new SelectList(movies, "MovieId", "Title");
            }
        }
    }
}
