using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class ScheDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<DayOfBusy> Chunks { get; set; }
    }
}
