using System;

namespace WebApi.Models
{
    public class DayOfBusy
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdRoom { get; set; }
        public TimeSpan TimeOfBusy { get; set; }
        public TimeSpan TimeOfFree { get; set; }
        public string Holder { get; set; }
        public string Note { get; set; }
    }
}