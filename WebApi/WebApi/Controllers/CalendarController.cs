using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/calendar")]
    public class CalendarController : Controller
    {
        DateTime now = DateTime.Now;
        StudentsContext db;
        long oneDay = (new DateTime(1, 1, 2)).Ticks;

        public CalendarController(StudentsContext context)
        {
            db = context;
            now -= now.TimeOfDay;
        }

        [HttpGet]
        public IActionResult GetScheduler(DateTime? startDay = null, DateTime? endDay = null)
        {
            if (startDay == null)
                startDay = new DateTime(now.Ticks - 14 * oneDay);

            if (endDay == null)
                endDay = new DateTime(now.Ticks + 14 * oneDay);

            List<DayOfBusy> weeks = new List<DayOfBusy>();
            long date = startDay.Value.Ticks;

            while (date <= endDay.Value.Ticks)
            {
                List<DayOfBusy> dayInDb = db.DaysOfBusy.Where(x => x.Date.Ticks == date).ToList();

                if (dayInDb.Count > 0)
                    for (int j = 0; j < dayInDb.Count; j++)
                        weeks.Add(dayInDb[j]);
                else
                    weeks.Add(new DayOfBusy { Date = new DateTime(date) });

                date += oneDay;
            }

            return Ok(weeks);
        }

        [HttpGet("{date; idRoom; tBusy}")]
        public IActionResult GetDay(DateTime date, int idRoom, TimeSpan tBusy)
        {
            DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == date
                                                                    && x.IdRoom == idRoom
                                                                    && x.TimeOfBusy == tBusy);

            if (dayOfBusy == null) return NotFound();

            return Ok(dayOfBusy);
        }

        [HttpPost]
        public IActionResult Post([FromBody]DayOfBusy dayOfBusy)
        {
            if (db.DaysOfBusy.FirstOrDefault(x => x == dayOfBusy) != null)
                return BadRequest("Day already exists!");

            if (db.MeetingRooms.Find(dayOfBusy.IdRoom) == null)
                return NotFound("Meeting room not found!");

            db.DaysOfBusy.Add(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpPut]
        public IActionResult Update([FromBody]DayOfBusy dayOfBusy)
        {
            if (db.DaysOfBusy.FirstOrDefault(x => x == dayOfBusy) != null)
                return Ok(dayOfBusy);

            DayOfBusy day = db.DaysOfBusy.FirstOrDefault(x => x.Date == dayOfBusy.Date
                                                                        && x.IdRoom == dayOfBusy.Id
                                                                        && x.TimeOfBusy == dayOfBusy.TimeOfBusy);

            if ( day == null) return NotFound("Day not found!");

            day = dayOfBusy;
            db.DaysOfBusy.Update(day);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpDelete("{date; idRoom; tBusy;}")]
        public IActionResult Delete(DateTime date, int idRoom, TimeSpan tBusy)
        {
            DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == date
                                                                    && x.IdRoom == idRoom
                                                                    && x.TimeOfBusy == tBusy);

            if (dayOfBusy == null) return NotFound("Day not found!");

            db.DaysOfBusy.Remove(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }
    }
}
