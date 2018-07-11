using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/calendar")]
    public class CalendarController : Controller
    {
        StudentsContext db;

        public CalendarController(StudentsContext context)
        {
            db = context;
        }

        DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        ShortInfoDay RandomDay()
        {
            Random r = new Random();

            return new ShortInfoDay
            {
                IdRoom = r.Next(1, 5),
                TimeOfBusy = new TimeSpan(r.Next(8, 21), r.Next(0, 60), 0),
                TimeOfFree = new TimeSpan(r.Next(8, 21), r.Next(0, 60), 0)
            };
        }

        [HttpGet("/getscheduler")]
        public IActionResult GetScheduler(DateTime? startDay = null, DateTime? endDay = null)
        {
            //DateTime now = DateTime.Now; now -= now.TimeOfDay;
            //long oneDay = (new DateTime(1, 1, 2)).Ticks;

            //if (startDay == null)
            //    startDay = new DateTime(now.Ticks - 14 * oneDay);

            //if (endDay == null)
            //    endDay = new DateTime(now.Ticks + 14 * oneDay);

            List<ShortInfoDay> weeks = new List<ShortInfoDay>();
            //long date = startDay.Value.Ticks;

            //while (date <= endDay.Value.Ticks)
            //{
            //    List<DayOfBusy> daysInDb = db.DaysOfBusy.Where(x => x.Date.Ticks == date).ToList();

            //    if (daysInDb.Count > 0)
            //        for (int j = 0; j < daysInDb.Count; j++)
            //            weeks.Add(daysInDb[j]);
            //    else
            //        weeks.Add(new DayOfBusy { Date = new DateTime(date) });

            //    date += oneDay;
            //}

            DateTime today = DateTime.Today;
            DateTime twoWeeksAgo = today.AddDays(-14);
            DateTime twoWeeksAhead = today.AddDays(14);
            DateTime strtDay = StartOfWeek(twoWeeksAgo, DayOfWeek.Monday);
            DateTime finDay = StartOfWeek(twoWeeksAhead, DayOfWeek.Monday).AddDays(7);

            while (strtDay < finDay)
            {
                weeks.Add(RandomDay());
                strtDay = strtDay.AddDays(1);
            }

            return Ok(weeks);
        }

        [HttpGet("/getday")]
        public IActionResult GetDay(DayOfBusy day)
        {
            //DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == day.Date
            //                                                        && x.IdRoom == day.IdRoom
            //                                                        && x.TimeOfBusy == day.TimeOfBusy);

            //if (dayOfBusy == null) return NotFound("Day not found!");

            return Ok(day);
        }

        [HttpPost]
        public IActionResult Post([FromBody]DayOfBusy dayOfBusy)
        {
            var qwe = db.ScheDays
                .Include(_=>_.Chunks)
                .First();

            //if (db.DaysOfBusy.Any(x => x.Date == dayOfBusy.Date
            //                           && x.IdRoom == dayOfBusy.IdRoom
            //                           && ((x.TimeOfBusy <= dayOfBusy.TimeOfBusy || x.TimeOfBusy > dayOfBusy.TimeOfBusy)
            //                                && (x.TimeOfFree <= dayOfBusy.TimeOfFree || x.TimeOfFree > dayOfBusy.TimeOfFree))))
            //    return BadRequest("Meeting room already exists!");
            //bool a = db.DaysOfBusy.Any(x => x.Date == dayOfBusy.Date),
            //    b = db.DaysOfBusy.Any(x => x.IdRoom == dayOfBusy.IdRoom),
            //    c = db.DaysOfBusy.Any(x => x.TimeOfBusy <= dayOfBusy.TimeOfBusy || x.TimeOfBusy > dayOfBusy.TimeOfBusy),
            //    d = db.DaysOfBusy.Any(x => x.TimeOfFree <= dayOfBusy.TimeOfFree || x.TimeOfFree > dayOfBusy.TimeOfFree);


            //if (db.MeetingRooms.Find(dayOfBusy.IdRoom) == null)
            //    return NotFound("Meeting room not found!");
            //if (true)
            //    return BadRequest();

            //db.DaysOfBusy.Add(dayOfBusy);
            //db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpPut]
        public IActionResult Update([FromBody]DayOfBusy dayOfBusy)
        {
            //if (db.DaysOfBusy.FirstOrDefault(x => x == dayOfBusy) != null)
            //    return Ok(dayOfBusy);

            //DayOfBusy day = db.DaysOfBusy.FirstOrDefault(x => x.Date == dayOfBusy.Date
            //                                                            && x.IdRoom == dayOfBusy.Id
            //                                                            && x.TimeOfBusy == dayOfBusy.TimeOfBusy);

            //if ( day == null) return NotFound("Day not found!");

            //day = dayOfBusy;
            //db.DaysOfBusy.Update(day);
            //db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpDelete]
        public IActionResult Delete(DayOfBusy item)
        {
            //DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == item.Date
            //                                                        && x.IdRoom == item.IdRoom
            //                                                        && x.TimeOfBusy == item.TimeOfBusy);

            //if (dayOfBusy == null) return NotFound("Day not found!");

            //db.DaysOfBusy.Remove(dayOfBusy);
            //db.SaveChanges();

            return Ok(item);
        }
    }
}
