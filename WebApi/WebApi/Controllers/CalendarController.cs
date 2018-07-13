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

        /// <summary>
        /// Получение недель по заданному интервалу
        /// </summary>
        /// <param name="startDay">Дата в первой недели</param>
        /// <param name="endDay">Дата в последней недели</param>
        /// <returns>Коллекция расписаний по дням</returns>
        [HttpGet("/getscheduler")]
        public IActionResult GetScheduler(DateTime? startDay = null, DateTime? endDay = null)
        {
            if (startDay == null && endDay == null)
            {
                DateTime today = DateTime.Today;
                startDay = today.AddDays(-14);
                endDay = today.AddDays(14);
            }

            if (startDay == null && endDay != null) startDay = endDay; //если 1 из параметров не задан, приравнивает его к заданному

            if (startDay != null && endDay == null) endDay = startDay;

            if (startDay > endDay) return BadRequest("Invalid date range!");

            startDay = StartOfWeek(startDay.Value, DayOfWeek.Monday); //первый день интервала
            endDay = StartOfWeek(endDay.Value, DayOfWeek.Monday).AddDays(7); //последний день интервала + 1
            List<ShortInfoDay> weeks = new List<ShortInfoDay>();

            while (startDay < endDay)
            {
                ScheDay scheDay = GetScheDay(startDay.Value);
                ShortInfoDay day = new ShortInfoDay(startDay.Value);
                day.CurrentWeek = IsCurrentWeek(startDay.Value);

                if (scheDay != null && scheDay.Chunks.Count > 0)
                {
                    day.CountRes = scheDay.Chunks.Count;
                    weeks.Add(day);
                }
                else
                    weeks.Add(day);

                startDay = startDay.Value.AddDays(1);
            }

            return Ok(weeks);
        }

        /// <summary>
        /// Получение расписания занятой переговорной
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="roomId">Номер переговорной</param>
        /// <returns>Расписание/returns>
        [HttpGet("/getday")]
        public IActionResult GetDay(DateTime date, int? roomId = null)
        {
            ScheDay scheDay = GetScheDay(date);

            if (scheDay == null) return NotFound("Date is free!");

            if (roomId != null && IsRoomNotExists(roomId.Value))
                return BadRequest("Room not found!");

            List<DayOfBusy> days = roomId == null
                ? scheDay.Chunks
                : scheDay.Chunks.FindAll(x => x.RoomId == roomId); //находим все записи по номеру переговорной

            if (date == DateTime.Today)
                foreach (DayOfBusy day in days)
                {
                    day.CurrentDay = true;
                    day.CurrentWeek = IsCurrentWeek(date);
                }

            return Ok(days);
        }

        /// <summary>
        /// Добавление записи о занятой переговорной
        /// </summary>
        /// <param name="dayOfBusy">Информация о переговорной</param>
        /// <param name="date">Дата</param>
        /// <returns>Сообщение о выполнении</returns>
        [HttpPost]
        public IActionResult AddDay([FromBody]DayOfBusy dayOfBusy, DateTime? date = null)
        {
            bool invalid;
            IActionResult request = IsDayInvalid(dayOfBusy, out invalid);

            if (invalid) return request;

            if (date == null) date = DateTime.Today;

            ScheDay scheDay = GetScheDay(date.Value);

            if (scheDay == null)
            {
                db.ScheDays.Add(new ScheDay
                {
                    Date = date.Value
                });
                db.SaveChanges();
                scheDay = GetScheDay(date.Value);
            } //если указанной даты нет, добавляем её в БД

            if (IsTimeIntervalInvalid(scheDay.Chunks, dayOfBusy))
                return BadRequest("Room already exists!");

            dayOfBusy.ScheDayId = scheDay.Id;
            db.DaysOfBusy.Add(dayOfBusy);
            db.SaveChanges();

            return Ok(dayOfBusy);
        }

        /// <summary>
        /// Обновление записи БД
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="oldDay">Старые данные</param>
        /// <param name="newDay">Новые данные</param>
        /// <returns>Сообщение о выполнении</returns>
        [HttpPut]
        public IActionResult Update(DateTime date, [FromBody]DayOfBusy oldDay, [FromBody]DayOfBusy newDay)
        {
            if (oldDay == null || newDay == null) return BadRequest("Day can not be null!");

            if (IsRoomNotExists(newDay.RoomId)) return NotFound("Room not found!");

            ScheDay scheDay = GetScheDay(date);

            if (scheDay == null) return NotFound("There are no occupied rooms for this date!");

            oldDay = scheDay.Chunks
                .FirstOrDefault(x => x.RoomId == oldDay.RoomId
                                     && x.TimeOfBusy == oldDay.TimeOfBusy
                                     && x.TimeOfFree == oldDay.TimeOfFree
                                     && x.Holder == oldDay.Holder);

            if (oldDay == null) return BadRequest("There is no such schedule!");

            newDay.ScheDayId = oldDay.ScheDayId;
            oldDay = newDay;

            db.DaysOfBusy.Update(oldDay);
            db.SaveChanges();

            return Ok(newDay);
        }

        /// <summary>
        /// Удаление записи о занятой переговорной
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="idRoom">Номер переговорной</param>
        /// <param name="tBusy">Время занятия переговорной</param>
        /// <returns>Сообщение о выполнении</returns>
        [HttpDelete]
        public IActionResult Delete(DateTime date, int idRoom, TimeSpan tBusy)
        {
            ScheDay scheDay = GetScheDay(date);

            if (scheDay == null)
                return NotFound("Day is not busy!");

            if (IsRoomNotExists(idRoom)) return NotFound("Room not found!");

            DayOfBusy day = scheDay.Chunks
                .FirstOrDefault(x => x.RoomId == idRoom
                                     && x.TimeOfBusy == tBusy);

            if (day == null)
                return NotFound("Room is already free!");

            db.DaysOfBusy.Remove(day);
            db.SaveChanges();

            scheDay = GetScheDay(date);

            if (scheDay.Chunks.Count == 0)
            {
                db.ScheDays.Remove(scheDay);
                db.SaveChanges();
            }

            return Ok(date);
        }

        /// <summary>
        /// Вычисляет дату начала недели, в которую входит указаная дата
        /// </summary>
        /// <param name="dt">Дата в недели</param>
        /// <param name="startOfWeek">День недели, с которого она начинается</param>
        /// <returns>Дата начала недели</returns>
        DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Получает расписание переговорных, занятых в указанную дату
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Расписание занятых переговорных</returns>
        ScheDay GetScheDay(DateTime date)
        {
            return db.ScheDays
                .Include(x => x.Chunks)
                .FirstOrDefault(x => x.Date == date);
        }

        /// <summary>
        /// Возвращает ответ, находится ли дата в текущей недели
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns>Ответ</returns>
        bool IsCurrentWeek(DateTime date)
        {
            DateTime startWeek = StartOfWeek(DateTime.Today, DayOfWeek.Monday),
                     endWeek = startWeek.AddDays(7);

            return date >= startWeek && date < endWeek;
        }

        /// <summary>
        /// Проверяет существует ли запись о переговорной в БД
        /// </summary>
        /// <param name="id">Номер переговорной</param>
        /// <returns>Ответ</returns>
        bool IsRoomNotExists(int id)
        {
            return db.MeetingRooms
                .FirstOrDefault(x => x.Id == id) == null;
        }

        IActionResult IsDayInvalid(DayOfBusy dayOfBusy, out bool flag)
        {
            if (flag = dayOfBusy == null) return BadRequest("Day can not be null!");

            if (flag = dayOfBusy.Holder == null) return BadRequest("Holder can not be null!");

            if (flag = IsRoomNotExists(dayOfBusy.RoomId)) return NotFound("Room not found!");

            if (flag = dayOfBusy.TimeOfBusy == dayOfBusy.TimeOfFree) return BadRequest("Invalid time interval!");

            if (flag = dayOfBusy.TimeOfBusy < new TimeSpan(8, 0, 0) || dayOfBusy.TimeOfBusy >= new TimeSpan(20, 0, 0)
                       || dayOfBusy.TimeOfFree <= new TimeSpan(8, 0, 0) || dayOfBusy.TimeOfFree > new TimeSpan(20, 0, 0))
                return BadRequest("Room can be occupied only during working hours!");

            return Ok(dayOfBusy);
        }

        bool IsTimeIntervalInvalid(List<DayOfBusy> chunks, DayOfBusy day)
        {
            return chunks.Any(x => x.RoomId == day.RoomId
                                    && day.TimeOfBusy >= x.TimeOfBusy
                                        && (day.TimeOfBusy <= x.TimeOfFree
                                        && day.TimeOfFree >= x.TimeOfFree || day.TimeOfFree <= day.TimeOfFree)
                                    || day.TimeOfBusy <= x.TimeOfBusy
                                        && (day.TimeOfFree >= x.TimeOfBusy
                                        && day.TimeOfFree <= x.TimeOfFree || day.TimeOfFree >= x.TimeOfFree));
        }
    }
}