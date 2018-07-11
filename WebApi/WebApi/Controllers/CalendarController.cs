﻿using System;
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
                ScheDay scheDay = db.ScheDays
                    .Include(x => x.Chunks)
                    .FirstOrDefault(x => x.Date == startDay);

                if (scheDay != null) //если дата есть в БД, берёт запись БД
                    foreach(DayOfBusy day in scheDay.Chunks)
                        weeks.Add(CutInfo(day, startDay.Value));
                else
                    weeks.Add(CutInfo(new DayOfBusy(), startDay.Value));

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
        public IActionResult GetDay(DateTime date, int? roomId)
        {
            ScheDay scheDay = GetScheDay(date);

            if (scheDay == null) return NotFound("Date is free!");

            if (roomId != null && db.MeetingRooms.Find(roomId.Value) == null)
                return BadRequest("Meeting room not found!");

            List<DayOfBusy> days = roomId == null
                ? scheDay.Chunks
                : scheDay.Chunks.FindAll(x => x.RoomId == roomId); //находим все записи по номеру переговорной

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
            if (dayOfBusy == null) return BadRequest("Day can not be null!");

            TimeSpan strtDay = new TimeSpan(8, 0, 0),
                     endDay = new TimeSpan(20, 0, 0);

            if ((dayOfBusy.TimeOfBusy < strtDay && dayOfBusy.TimeOfBusy >= endDay)
                || (dayOfBusy.TimeOfFree <= strtDay && dayOfBusy.TimeOfFree > endDay))
                return BadRequest("Room can be occupied only during working hours!");

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
        public IActionResult Update(DateTime date, [FromBody]DayOfBusy oDay, [FromBody]DayOfBusy nDay)
        {
            return Ok();
            //if (dayOfBusy == null)
            //    return BadRequest("Day can not be null!");

            //ScheDay scheDay = GetScheDay(date);

            //if (scheDay == null)
            //    return NotFound("Day is not busy!");

            //if (scheDay.Chunks
            //    .FirstOrDefault(x => ))

            //return Ok(dayOfBusy);
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

            DayOfBusy day = scheDay.Chunks
                .FirstOrDefault(x => x.RoomId == idRoom
                                     && x.TimeOfBusy == tBusy);

            if (day == null)
                return NotFound("Room is already free!");

            db.DaysOfBusy.Remove(day);
            db.SaveChanges();

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
        /// Обрезает информацию о заданном занятой переговорной
        /// </summary>
        /// <param name="day">Информация о занятой переговорной</param>
        /// <param name="date">Дата дня</param>
        /// <returns>Обрезанная информация</returns>
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
    }
}