using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/mrs")]
    public class MeetingRoomsController : Controller
    {
        StudentsContext db;

        public MeetingRoomsController(StudentsContext context)
        {
            db = context;

            if (db.MeetingRooms.Count() == 0)
            {
                db.MeetingRooms.Add(new MeetingRoom { Id = 1 });
                db.MeetingRooms.Add(new MeetingRoom { Id = 2 });
                db.SaveChanges();
            }
        }

        [HttpGet("/getall")]
        public IActionResult Get()
        {
            return Ok(db.MeetingRooms.ToList());
        }

        [HttpGet("/getone")]
        public IActionResult Get(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms.FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound();

            return Ok(meetingRoom);
        }

        [HttpPost]
        public IActionResult Post([FromBody]MeetingRoom meetingRoom)
        {
            if (meetingRoom == null) return BadRequest();

            db.MeetingRooms.Add(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }

        [HttpPut]
        public IActionResult Put([FromBody]MeetingRoom meetingRoom)
        {
            if (meetingRoom == null) return BadRequest();

            if (db.MeetingRooms.Any(x => x.Id == meetingRoom.Id)) return NotFound();

            db.Update(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }
        
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms.FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound();

            db.MeetingRooms.Remove(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }
    }
}