using DiiCommon.Time;
using System;
using System.ComponentModel.DataAnnotations;
using Dii_TheaterManagement_Web.Clients;
using Dii_TheaterManagement_Web.Clients;

namespace Dii_TheaterManagement_Web.Models
{
    public class MonthlySnackPackOrderViewModel
    {
        public MonthlySnackPackOrderViewModel()
        {
            Month = new Month(DateTime.Now);
        }

        public MonthlySnackPackOrderViewModel(int monthId, SnackPackOrder snackPackOrder)
            : this(new Month(monthId), snackPackOrder)
        {
        }

        public MonthlySnackPackOrderViewModel(Month month, SnackPackOrder snackPackOrder)
        {
            Month = month;
            if (snackPackOrder != null)
            {
                OrderExists = true;
                OrderCount = snackPackOrder.OrderCount;
            }
        }

        public Month Month { get; set; }
        public int MonthId
        {
            get
            {
                return Month.MonthId;
            }
            set
            {
                Month = new Month(value);
            }
        }

        public int OrderCount { get; set; }
        public bool OrderExists { get; set; }

        [Display(Name = "Month")]
        public string MonthDisplayValue => Month.DisplayValue;

        internal SnackPackOrder AsSnackPackOrder(string theaterCode)
        {
            return new SnackPackOrder()
            {
                MonthId = MonthId,
                OrderCount = OrderCount,
                TheaterCode = theaterCode
            };
        }
    }
}
