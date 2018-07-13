using System;
using System.Globalization;

namespace WebApi.Models
{
    public class ShortInfoDay
    {
        public ShortInfoDay(DateTime date)
        {
            Date = date;
            CurrentDay = date == DateTime.Today;
        }

        public int CountRes { get; set; }
        public DateTime Date { get; set; }
        public string Month
        {
            get { return Date.ToString("MMM", CultureInfo.InvariantCulture); }
        }
        public bool CurrentWeek { get; set; }
        public bool CurrentDay { get; set; }
    }
}
