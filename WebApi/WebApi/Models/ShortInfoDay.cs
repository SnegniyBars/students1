using System;

namespace WebApi.Models
{
    public class ShortInfoDay
    {
        public int IdRoom { get; set; }
        public TimeSpan TimeOfBusy { get; set; }
        public TimeSpan TimeOfFree { get; set; }
    }
}
