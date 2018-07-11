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

        ShortInfoDay CutInfo(DayOfBusy day, DateTime date)
        {
            DateTime startWeek = StartOfWeek(DateTime.Today, DayOfWeek.Monday),
                     endWeek = startWeek.AddDays(7);

            return new ShortInfoDay
            {
                IdRoom = day.RoomId,
                TimeOfBusy = day.TimeOfBusy,
                TimeOfFree = day.TimeOfFree,
                CurrentDay = date == DateTime.Today,
                CurrentWeek = date >= startWeek && date < endWeek
            };
        }

        [HttpGet("/getscheduler")]
        public IActionResult GetScheduler(DateTime? startDay = null, DateTime? endDay = null)
        {
            if (startDay == null && endDay == null)
            {
                DateTime today = DateTime.Today;
                startDay = today.AddDays(-14);
                endDay = today.AddDays(14);
            }

            if (startDay == null && endDay != null) startDay = endDay;

            if (startDay != null && endDay == null) endDay = startDay;

            if (startDay > endDay) return BadRequest("Invalid date range!");

            startDay = StartOfWeek(startDay.Value, DayOfWeek.Monday);
            endDay = StartOfWeek(endDay.Value, DayOfWeek.Monday).AddDays(7);
            List<ShortInfoDay> weeks = new List<ShortInfoDay>();

            while (startDay < endDay)
            {
                ScheDay scheDay = db.ScheDays
                    .Include(x => x.Chunks)
                    .FirstOrDefault(x => x.Date == startDay);
                DayOfBusy dayOfBusy = new DayOfBusy();

                if (scheDay != null && scheDay.Chunks.Count > 0)
                    for (int i = 0; i < scheDay.Chunks.Count; i++)
                    {
                        DayOfBusy day = db.DaysOfBusy
                            .FirstOrDefault(x => x == scheDay.Chunks[i]);
                        weeks.Add(CutInfo(day, startDay.Value));
                    }
                else
                    weeks.Add(CutInfo(dayOfBusy, startDay.Value));

                startDay = startDay.Value.AddDays(1);
            }

            return Ok(weeks);
        }

        [HttpGet("/getday")]
        public IActionResult GetDay(DateTime date, int? roomId)
        {
            List<DayOfBusy> days;
            ScheDay scheDay = db.ScheDays
                .Include(x => x.Chunks)
                .FirstOrDefault(x => x.Date == date);
            days = scheDay.Chunks;

            if (roomId != null)
                days = days.FindAll(x => x.Room.Id == roomId);

            if (scheDay == null) return NotFound("Date is free!");

            return Ok(days);
        }

        void AddScheDay(DateTime date)
        {
            ScheDay scheDay;

            db.ScheDays.Add(scheDay = new ScheDay
            {
                Date = date
            });
            db.SaveChanges();
        }

        [HttpPost]
        public IActionResult AddDay([FromBody]DayOfBusy dayOfBusy, DateTime? date = null)
        {
            if (dayOfBusy == null) return BadRequest("Day can not be null!");

            TimeSpan strtDay = new TimeSpan(8, 0, 0),
                     endDay = new TimeSpan(20, 0, 0);

            if ((dayOfBusy.TimeOfBusy < strtDay && dayOfBusy.TimeOfBusy >= endDay)
                || (dayOfBusy.TimeOfFree <= strtDay && dayOfBusy.TimeOfFree > endDay))
                return BadRequest("Room can be occupied only during working hours!");

            if (date == null)
                date = DateTime.Today;

            ScheDay scheDay = db.ScheDays
                .Include(x => x.Chunks)
                .FirstOrDefault(x => x.Date == date);

            if (scheDay == null)
            {
                AddScheDay(date.Value);
                scheDay = db.ScheDays
                .Include(x => x.Chunks)
                .FirstOrDefault(x => x.Date == date);
            }

            dayOfBusy.RoomId = dayOfBusy.Room.Id;

            if (db.MeetingRooms
                .FirstOrDefault(x => x.Id == dayOfBusy.RoomId) == null)
                return BadRequest("Invalid room!");

            if (scheDay.Chunks
                .Any(x => x.RoomId == dayOfBusy.RoomId
                          && x.TimeOfBusy <= dayOfBusy.TimeOfBusy
                             || x.TimeOfBusy > dayOfBusy.TimeOfBusy
                          && x.TimeOfFree <= dayOfBusy.TimeOfFree
                             || x.TimeOfFree > dayOfBusy.TimeOfFree))
                return BadRequest("Room already exists!");

            dayOfBusy.ScheDay = scheDay;
            dayOfBusy.ScheDayId = scheDay.Id;
            db.DaysOfBusy.Add(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpPut]
        public IActionResult Update(DateTime date, [FromBody]DayOfBusy dayOfBusy)
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
        public IActionResult Delete(DateTime date, int idRoom, TimeSpan tBusy)
        {
            //DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == item.Date
            //                                                        && x.IdRoom == item.IdRoom
            //                                                        && x.TimeOfBusy == item.TimeOfBusy);

            //if (dayOfBusy == null) return NotFound("Day not found!");

            //db.DaysOfBusy.Remove(dayOfBusy);
            //db.SaveChanges();

            return Ok(date);
        }
    }
}