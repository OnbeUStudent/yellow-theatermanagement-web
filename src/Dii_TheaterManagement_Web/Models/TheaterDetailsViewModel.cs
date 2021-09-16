using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dii_TheaterManagement_Web.Clients;

namespace Dii_TheaterManagement_Web.Models
{
    public class TheaterDetailsViewModel
    {
        public TheaterDetailsViewModel()
        {
        }

        public TheaterDetailsViewModel(DetailedTheater detailedTheater)
        {
            TheaterName = detailedTheater.TheaterName;
            AddressLine1 = detailedTheater.AddressLine1;
            AddressLine2 = detailedTheater.AddressLine2;
            City = detailedTheater.City;
            State = detailedTheater.State;
            Zip = detailedTheater.Zip;
            ThemeName = detailedTheater.WebSiteTheme.Name;
        }

        public string TheaterName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ThemeName { get; set; }

        internal DetailedTheater AsDetailedTheater()
        {
            return new DetailedTheater()
            {
                TheaterName = TheaterName,
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                City = City,
                State = State,
                Zip = Zip,
                WebSiteTheme = new WebSiteTheme() { Name = ThemeName }
            };
        }
    }
}
