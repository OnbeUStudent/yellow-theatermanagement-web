﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeTheaterBff.Data;
using FakeTheaterBff.Features.SyntheticBehavior;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//
namespace FakeTheaterBff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly FakeTheaterBffContext _context;
        private readonly UserInfoAccessor _userInfoAccessor;

        public BookingsController(FakeTheaterBffContext context, UserInfoAccessor userInfoAccessor)
        {
            _context = context;
            _userInfoAccessor = userInfoAccessor;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var bookings = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.TheaterCode == theaterCode)
                .ToListAsync();

            if (bookings.Any())
            {
                // We can't use .Include(booking => booking.Movies) because of circular references so we
                // do the following to include the movie info in our response.
                var movieIds = bookings.Select(b => b.MovieId).Distinct();
                var movies = await _context.Movies
                    .Include(movie => movie.MovieMetadata)
                    .Where(movie => movieIds.Contains(movie.MovieId))
                    .ToListAsync();
                foreach (var movie in movies)
                {
                    movie.Bookings = null;
                }
                foreach (var booking in bookings)
                {
                    booking.Movie = movies.Single(movie => booking.MovieId == movie.MovieId);
                }
            }
            return bookings;
        }

        // GET: api/Bookings/5
        [HttpGet("{monthId}")]
        public async Task<ActionResult<Booking>> GetBooking(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var booking = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(b => b.MonthId == monthId);

            if (booking == null)
            {
                return NotFound();
            }

            // We can't use .Include(booking => booking.Movies) because of circular references so we
            // do the following to include the movie info in our response.
            var movie = await _context.Movies
                .AsNoTracking()
                .Include(movie => movie.MovieMetadata)
                .SingleAsync(movie => movie.MovieId == booking.MovieId);
            movie.Bookings = null;
            booking.Movie = movie;

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> PutBooking(int monthId, Booking booking)
        {
            if (monthId != booking.MonthId)
            {
                return BadRequest();
            }
            string theaterCode = _userInfoAccessor.TheaterCode;
            booking.TheaterCode = theaterCode;

            try
            {
                var oldBooking = _context.Bookings.SingleOrDefault(b => b.MonthId == monthId && b.TheaterCode == theaterCode);
                if (oldBooking != null)
                {
                    _context.Bookings.Remove(oldBooking);
                }
                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(theaterCode, monthId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{monthId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteBooking(int monthId)
        {
            string theaterCode = _userInfoAccessor.TheaterCode;
            var booking = await _context.Bookings
                .Where(b => b.TheaterCode == theaterCode)
                .SingleOrDefaultAsync(b => b.MonthId == monthId);
            if (booking == null)
            {
                return NotFound();
            }
            booking.TheaterCode = theaterCode;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(string theaterCode, int monthId)
        {
            return _context.Bookings
                .Any(e => e.TheaterCode == theaterCode && e.MonthId == monthId);
        }
    }
}
