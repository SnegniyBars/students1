using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ScheDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<DayOfBusy> Chunks { get; set; }
    }
}
