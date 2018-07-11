using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace WebApi.Models
{
    public class DayOfBusy
    {
        [Key]
        public int Id { get; set; }
        
        public MeetingRoom Room { get; set; }
        public TimeSpan TimeOfBusy { get; set; }
        public TimeSpan TimeOfFree { get; set; }
        public string Holder { get; set; }
        public string Note { get; set; }
        public bool CurrentWeek { get; set; }
        public bool CurrentDay { get; set; }

        [ForeignKey("ScheDay")]
        public int ScheDayId { get; set; }

        public virtual ScheDay ScheDay { get; set; }
    }
}