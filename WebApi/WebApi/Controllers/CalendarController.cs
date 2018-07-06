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
        List<List<DateTime>> weeks = new List<List<DateTime>>();
        DateTime now = DateTime.Now;
        StudentsContext db;
        long oneDay;


        public CalendarController(StudentsContext context)
        {
            db = context;
            DateTime a = new DateTime(),
                     b = new DateTime(1, 1, 2);
            oneDay = b.Subtract(a).Ticks;
        }

        [HttpPost]
        public IActionResult GetScheduler(DateTime? startDay = null, DateTime? endDay = null)
        {
            if (startDay == null) startDay = new DateTime((DateTime.Now - DateTime.Now.TimeOfDay).Ticks - 14 * oneDay);

            //if (endDay == null) startDay = new DateTime((DateTime.Now - DateTime.Now.TimeOfDay).Ticks + 14 * oneDay);

            for (int i = 0; i < 5; i++)
            {
                weeks.Add(new List<DateTime>());

                for (int j = 0; j < 7; j++)
                    weeks[i].Add(new DateTime(startDay.Value.Ticks + oneDay));
            }

            return Ok(weeks);
        }

        [HttpGet("{day; month; year}")]
        public IActionResult GetDay(int day, int month, int year)
        {
            DateTime date = new DateTime(year, month, day);

            return Ok(date);
        }

        [HttpPost]
        public IActionResult Post([FromBody]DayOfBusy dayOfBusy)
        {
            if (db.DaysOfBusy.FirstOrDefault(x => x == dayOfBusy) != null)
                return BadRequest();

            db.DaysOfBusy.Add(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]DayOfBusy dayOfBusy)
        {
            if (db.DaysOfBusy.FirstOrDefault(x => x == dayOfBusy) == null)
                return BadRequest();

            db.DaysOfBusy.Update(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(DateTime date)
        {
            DayOfBusy dayOfBusy = db.DaysOfBusy.FirstOrDefault(x => x.Date == date);

            if (dayOfBusy == null) return BadRequest();

            db.DaysOfBusy.Remove(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }
    }
}
