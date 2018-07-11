using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MeetingRoomsController : Controller
    {
        StudentsContext db;

        public MeetingRoomsController(StudentsContext context)
        {
            db = context;
        }

        /// <summary>
        /// Получение списка всех переговорных
        /// </summary>
        /// <returns>Список переговорных</returns>
        [HttpGet("/getAllRooms")]
        public IActionResult GetAll()
        {
            return Ok(db.MeetingRooms.ToList());
        }

        /// <summary>
        /// Получение переговорной по номеру в БД
        /// </summary>
        /// <param name="id">Номер переговорной в БД</param>
        /// <returns>Переговорная</returns>
        [HttpGet("/getOneRoom")]
        public IActionResult GetOne(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms.FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound();

            return Ok(meetingRoom);
        }

        /// <summary>
        /// Добавление переговорной в БД
        /// </summary>
        /// <returns>Сообщение о выполнении</returns>
        [HttpPost]
        public IActionResult AddRoom()
        {
            MeetingRoom meetingRoom = new MeetingRoom { };

            db.MeetingRooms.Add(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }

        /// <summary>
        /// Обновление записи БД о переговорной
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Сообщение о выполнении</returns>
        [HttpPut]
        public IActionResult Put(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms
                .FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound("Meeting room not found!");

            db.Update(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }

        /// <summary>
        /// Удаление записи БД о переговорной
        /// </summary>
        /// <param name="id">Номер переговорной</param>
        /// <returns>Сообщение о выполнении</returns>
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            MeetingRoom meetingRoom = db.MeetingRooms
                .FirstOrDefault(x => x.Id == id);

            if (meetingRoom == null) return NotFound("Meeting room not found!");

            db.MeetingRooms.Remove(meetingRoom);
            db.SaveChanges();

            return Ok(meetingRoom);
        }
    }
}