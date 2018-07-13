using System;

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
        public bool CurrentWeek { get; set; }
        public bool CurrentDay { get; set; }
    }
}
