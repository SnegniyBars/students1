using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class MeetingRoom
    {
        [Key]
        public virtual int Id { get; set; }
    }
}