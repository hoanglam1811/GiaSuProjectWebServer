using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;
using System.Linq;

namespace PRN231.API.Controllers.OData
{
    public class BookingUserODataController : ODataController
    {
        private readonly IGenericService<BookingUser, BookingUserDTO> _bookingUserService;
        private readonly IGenericRepository<BookingUser> _bookingUserRepo;

        public BookingUserODataController(
            IGenericRepository<BookingUser> bookingUserRepo,
            IGenericService<BookingUser, BookingUserDTO> bookingUserService)
        {
            _bookingUserRepo = bookingUserRepo;
            _bookingUserService = bookingUserService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<BookingUser>>> Get()
        {
            var bookingUserList = await _bookingUserRepo.GetAll();
            return Ok(bookingUserList);
        }

        [EnableQuery]
        public async Task<ActionResult<BookingUser>> Get(int key)
        {
            var bookingUser = await _bookingUserService.Get(key);
            return bookingUser != null ? Ok(bookingUser) : NotFound();
        }

        public async Task<ActionResult<BookingUser>> Post([FromBody] BookingUserDTO dto)
        {
            var bookingUser = await _bookingUserService.Add(dto);
            return Created(bookingUser);
        }

        public async Task<IActionResult> Put(int key, [FromBody] BookingUserDTO dto)
        {

            var bookingUser = await _bookingUserService.Update(dto);
            return Updated(bookingUser);
        }

       public async Task<IActionResult> Delete(int key)
        {
            var bookingUser = await _bookingUserService.Get(key);
            if (bookingUser == null) return NotFound();

            await _bookingUserService.Delete(key);
            return NoContent();
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<BookingUser>>> GetBookingUserByUserId(int id)
        {
            var bookingUserList = await _bookingUserRepo.GetAll(x => x.Include(a => a.User));
            bookingUserList = bookingUserList.Where(c => c.UserId == id);
            return Ok(bookingUserList);
        }
    }
}
