using DiiCommon.Time;
using System;
using System.ComponentModel.DataAnnotations;
using Dii_TheaterManagement_Web.Clients;

namespace Dii_TheaterManagement_Web.Models
{
    /// <summary>
    /// Represents zero or one of the Bookings associated with a Theater for a particular Month.
    /// If there is no Booking for this Month, <see cref="Booking"/> will be null;
    /// </summary>
    public class TheaterBookingViewModel
    {
        private Month _month;

        public TheaterBookingViewModel()
        {
        }

        public TheaterBookingViewModel(Booking booking, int monthId)
            : this(booking, new Month(monthId))
        {
        }

        public TheaterBookingViewModel(Booking booking, Month month)
        {
            _month = month;
            if (booking != null)
            {
                HasBooking = true;
                Movie = booking.Movie;
                MovieId = booking.MovieId;
            }
        }

        public bool HasBooking { get; }

        /// <summary>
        /// One of three keys that make up <see cref="Booking"/>'s composite key.
        /// </summary>
        /// <remarks>
        /// <see cref="TheaterBookingViewModel"/> has only two of these three keys.
        /// The third key, <see cref="Booking.TheaterId"/>, isn't 
        /// <see cref="TheaterBookingViewModel"/>'s responsibility.
        /// </remarks>
        [Display(Name = "Movie")]
        public long MovieId { get; set; }
        public Movie Movie { get; set; }

        /// <summary>
        /// One of three keys that make up <see cref="Booking"/>'s composite key.
        /// </summary>
        /// <remarks>
        /// <see cref="TheaterBookingViewModel"/> has only two of these three keys.
        /// The third key, <see cref="Booking.TheaterId"/>, isn't 
        /// <see cref="TheaterBookingViewModel"/>'s responsibility.
        /// </remarks>
        [Display(Name = "Month")]
        public int MonthId
        {
            get
            {
                return _month.MonthId;
            }
            set
            {
                _month = new Month(value);
            }
        }

        [Display(Name = "Month")]
        public string MonthDisplayValue => _month.DisplayValue;

        internal Booking AsBooking()
        {
            return new Booking()
            {
                MonthId = MonthId,
                MovieId = MovieId
            };
        }
    }
}
