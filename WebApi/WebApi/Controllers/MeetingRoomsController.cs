using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                db.MeetingRooms.Add(new MeetingRoom { });
                db.MeetingRooms.Add(new MeetingRoom { });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<MeetingRoom> Get()
        {
            return db.MeetingRooms.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms.FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound();

            return new ObjectResult(meetingRoom);
        }

        [HttpPost]
        public IActionResult Post([FromBody]MeetingRoom meetingRoom)
        {
            if (meetingRoom == null) return BadRequest();

            db.MeetingRooms.Add(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]MeetingRoom meetingRoom)
        {
            if (meetingRoom == null) return BadRequest();

            if (db.MeetingRooms.Any(x => x.Id == meetingRoom.Id)) return NotFound();

            db.Update(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }
        
        [HttpDelete("{id}")]
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