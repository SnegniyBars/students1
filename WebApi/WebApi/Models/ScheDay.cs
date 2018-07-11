using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class ScheDay
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<DayOfBusy> Chunks { get; set; }
    }
}
