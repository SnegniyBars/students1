using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class StudentsContext : DbContext
    {
        public StudentsContext(DbContextOptions<StudentsContext> options)
            : base(options)
        { }

        public DbSet<MeetingRoom> MeetingRooms { get; set; }
        public DbSet<DayOfBusy> DaysOfBusy { get; set; }
    }
}