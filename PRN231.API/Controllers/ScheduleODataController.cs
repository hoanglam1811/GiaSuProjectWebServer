using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore; // For Include

namespace PRN231.API.Controllers.OData
{
    public class ScheduleODataController : ODataController
    {
        private readonly IGenericService<Schedule, ScheduleDTO> _scheduleService;
        private readonly IGenericRepository<Schedule> _scheduleRepo;

        public ScheduleODataController(
            IGenericService<Schedule, ScheduleDTO> scheduleService,
            IGenericRepository<Schedule> scheduleRepo)
        {
            _scheduleService = scheduleService;
            _scheduleRepo = scheduleRepo;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Schedule>>> Get()
        {
            var scheduleList = await _scheduleService.GetAll();
            return Ok(scheduleList);
        }

        [EnableQuery]
        public async Task<ActionResult<Schedule>> Get(int key)
        {
            var schedule = await _scheduleService.Get(key);
            return schedule != null ? Ok(schedule) : NotFound();
        }

        public async Task<ActionResult<Schedule>> Post([FromBody] ScheduleDTO dto)
        {
            var schedule = await _scheduleService.Add(dto);
            return Created(schedule);
        }

        public async Task<IActionResult> Put(int key, [FromBody] ScheduleDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var schedule = await _scheduleService.Update(dto);
            return Updated(schedule);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var schedule = await _scheduleService.Get(key);
            if (schedule == null) return NotFound();

            await _scheduleService.Delete(key);
            return NoContent();
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Schedule>>> GetAllByUserId(int userId)
        {
            var scheduleList = await _scheduleRepo.GetAll(x => x.Include(x => x.Booking)
                  .ThenInclude(x => x.BookingUsers)
                  .ThenInclude(x => x.User)
                  , x => x.Include(x => x.Booking));
            scheduleList = scheduleList.Where(x =>
                  x.Booking.BookingUsers
                  .Any(y =>
                    y.UserId == userId));
            return Ok(scheduleList);
        }
    }
}
