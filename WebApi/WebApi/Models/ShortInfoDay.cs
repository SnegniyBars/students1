using System;
using System.Globalization;

namespace WebApi.Models
{
    public class ShortInfoDay
    {
        public int CountRes { get; set; }
        public DateTime Date { get; set; }
        public string Month { get { return Date.ToString("MMMMMMMM", CultureInfo.GetCultureInfo("ru-RU-Cyrl")); } }
        public bool CurrentDay { get { return Date == DateTime.Today; } }
        public bool Weekend { get { return Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday; } }
        public bool CurrentWeek { get; set; }
        public bool CurrentMonth { get { return Date.Month == DateTime.Today.Month; } }
    }
}
