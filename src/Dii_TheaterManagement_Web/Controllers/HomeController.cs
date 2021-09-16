using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using Dii_TheaterManagement_Web.Models;

namespace Dii_TheaterManagement_Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        //private readonly BffClient _bffClient;

        //public HomeController(ILogger<HomeController> logger, BffClient bffClient)
        //{
        //    _logger = logger;
        //    _bffClient = bffClient;
        //}

        public IActionResult Index()
        {
            //if (!User.TryGetTheaterInfo(_bffClient, out long theaterId, out string theaterCode)) { return View("NoAssignedTheater"); }

            List<Alert> alerts = new List<Alert>();
            //var currentMonth = new Month(DateTime.Now);
            //{
            //    var lastMonthInBookingWindow = currentMonth.Plus(DiiConstants.Theaters.BookingWindowSizeInMonths - 1);
            //    var monthsStartingAt = new MonthsStartingAt(currentMonth, DiiConstants.Theaters.BookingWindowSizeInMonths);

            //    var bookings = _bffClient.Bookings
            //        .AsNoTracking()
            //        .Where(booking => booking.TheaterId == theaterId)
            //        .Where(booking => currentMonth.MonthId <= booking.MonthId && booking.MonthId <= lastMonthInBookingWindow.MonthId);

            //    int dangerAlertCount = AddBookingAlerts(alerts, AlertType.Danger, bookings,
            //        currentMonth,
            //        currentMonth.Plus(DiiConstants.Theaters.BookingDangerAlertWindowSizeInMonths - 1));
            //    int warningAlertCount = AddBookingAlerts(alerts, AlertType.Warning, bookings,
            //        currentMonth.Plus(DiiConstants.Theaters.BookingDangerAlertWindowSizeInMonths),
            //        currentMonth.Plus(DiiConstants.Theaters.BookingDangerAlertWindowSizeInMonths + DiiConstants.Theaters.BookingWarningAlertWindowSizeInMonths - 1));
            //    int infoAlertCount = AddBookingAlerts(alerts, AlertType.Info, bookings,
            //        currentMonth.Plus(DiiConstants.Theaters.BookingDangerAlertWindowSizeInMonths + DiiConstants.Theaters.BookingWarningAlertWindowSizeInMonths),
            //        currentMonth.Plus(DiiConstants.Theaters.BookingWindowSizeInMonths - 1));

            //    if (dangerAlertCount == 0 && warningAlertCount == 0 && infoAlertCount == 0)
            //    {
            //        alerts.Add(new Alert()
            //        {
            //            AlertType = AlertType.Success,
            //            Heading = "Well done!",
            //            FirstPartOfContent = $"Your theater has movies booked through {lastMonthInBookingWindow.DisplayValue}",
            //            Remarks = "If you want to review your booked movies, use the links at the top of this page."
            //        }
            //        );
            //    }
            //}
            //{
            //    var lastMonthInSnackPackOrderWindow = currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingWindowSizeInMonths - 1);
            //    var monthsStartingAt = new MonthsStartingAt(currentMonth, DiiConstants.Theaters.SnackPackOrderingWindowSizeInMonths);

            //    var SnackPackOrders = _bffClient.SnackPackOrders
            //        .AsNoTracking()
            //        .Where(snackPackOrder => snackPackOrder.TheaterId == theaterId)
            //        .Where(snackPackOrder => currentMonth.MonthId <= snackPackOrder.MonthId && snackPackOrder.MonthId <= lastMonthInSnackPackOrderWindow.MonthId);

            //    int dangerAlertCount = AddSnackPackOrderAlerts(alerts, AlertType.Danger, SnackPackOrders,
            //        currentMonth,
            //        currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingDangerAlertWindowSizeInMonths - 1));
            //    int warningAlertCount = AddSnackPackOrderAlerts(alerts, AlertType.Warning, SnackPackOrders,
            //        currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingDangerAlertWindowSizeInMonths),
            //        currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingDangerAlertWindowSizeInMonths + DiiConstants.Theaters.SnackPackOrderingWarningAlertWindowSizeInMonths - 1));
            //    int infoAlertCount = AddSnackPackOrderAlerts(alerts, AlertType.Info, SnackPackOrders,
            //        currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingDangerAlertWindowSizeInMonths + DiiConstants.Theaters.SnackPackOrderingWarningAlertWindowSizeInMonths),
            //        currentMonth.Plus(DiiConstants.Theaters.SnackPackOrderingWindowSizeInMonths - 1));

            //    if (dangerAlertCount == 0 && warningAlertCount == 0 && infoAlertCount == 0)
            //    {
            //        alerts.Add(new Alert()
            //        {
            //            AlertType = AlertType.Success,
            //            Heading = "Well done!",
            //            FirstPartOfContent = $"Your theater has snack packs ordered through {lastMonthInSnackPackOrderWindow.DisplayValue}",
            //            Remarks = "If you want to review your snack pack orders, use the links at the top of this page."
            //        }
            //        );
            //    }
            //}
            return View(alerts);
        }

        //private int AddBookingAlerts(List<Alert> alerts, AlertType alertType, IQueryable<Booking> bookings, Month startingMonth, Month endingMonth)
        //{
        //    List<Month> monthsWithoutAnyBooking = new List<Month>();
        //    for (Month iMonth = startingMonth; iMonth.MonthId <= endingMonth.MonthId; iMonth = iMonth.Plus(1))
        //    {
        //        if (!bookings.Any(booking => booking.MonthId == iMonth.MonthId))
        //        {
        //            monthsWithoutAnyBooking.Add(iMonth);
        //        }
        //    }
        //    if (monthsWithoutAnyBooking.Any())
        //    {
        //        alerts.Add(new Alert()
        //        {
        //            AlertType = alertType,
        //            ShowBadge = true,
        //            FirstPartOfContent = 
        //                $"You haven't booked a movie for {string.Join(", ", monthsWithoutAnyBooking.Select(a => a.DisplayValue))}. Click ",
        //            LinkContent = "here",
        //            LinkHref = "/CurrentBookings",
        //            SecondPartOfContent = " to book movies."
        //        });
        //    }
        //    return monthsWithoutAnyBooking.Count();
        //}

        //private int AddSnackPackOrderAlerts(List<Alert> alerts, AlertType alertType, IQueryable<SnackPackOrder> SnackPackOrders, Month startingMonth, Month endingMonth)
        //{
        //    List<Month> monthsWithoutAnySnackPackOrder = new List<Month>();
        //    for (Month iMonth = startingMonth; iMonth.MonthId <= endingMonth.MonthId; iMonth = iMonth.Plus(1))
        //    {
        //        if (!SnackPackOrders.Any(SnackPackOrder => SnackPackOrder.MonthId == iMonth.MonthId))
        //        {
        //            monthsWithoutAnySnackPackOrder.Add(iMonth);
        //        }
        //    }
        //    if (monthsWithoutAnySnackPackOrder.Any())
        //    {
        //        alerts.Add(new Alert()
        //        {
        //            AlertType = alertType,
        //            ShowBadge = true,
        //            FirstPartOfContent =
        //                $"You haven't set a snack pack order for {string.Join(", ", monthsWithoutAnySnackPackOrder.Select(a => a.DisplayValue))}. Click ",
        //            LinkContent = "here",
        //            LinkHref = "/SnackPackOrders",
        //            SecondPartOfContent = " to order snack packs."
        //        });
        //    }
        //    return monthsWithoutAnySnackPackOrder.Count();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
