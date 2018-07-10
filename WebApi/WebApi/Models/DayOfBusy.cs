using System;

namespace WebApi.Models
{
    public class DayOfBusy
    {
        public int Id { get; set; }
        public MeetingRoom Room { get; set; }
        public TimeSpan TimeOfBusy { get; set; }
        public TimeSpan TimeOfFree { get; set; }
        public string Holder { get; set; }
        public string Note { get; set; }
        public bool CurrentWeek { get; set; }
        public bool CurrentDay { get; set; }
    }
}