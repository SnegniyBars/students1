using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class DayOfBusy
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual MeetingRoom Room { get; set; }
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