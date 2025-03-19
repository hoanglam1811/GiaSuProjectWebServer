using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Net;

namespace PRN231.API.Controllers.OData
{
    public class BookingsController : ODataController
    {
        private readonly IBookingService _bookingService;

        public BookingsController(
            IBookingService bookingService,
            IGenericService<User, UserDTO> userService)
        {
            _bookingService = bookingService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Booking>>> Get()
        {
            var bookings = await _bookingService.GetAllBookings();
            return Ok(bookings.AsQueryable());
        }

        public async Task<ActionResult<Booking>> Post([FromBody] CreateBookingRequest request)
        {
            try
            {
                var bookingResponse = await _bookingService.CreateBooking(request);
                return Created(bookingResponse);
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        public async Task<IActionResult> Put(int key, [FromBody] UpdateBookingRequest request)
        {
            try
            {
                if (key != request.BookingId) return BadRequest();

                var bookingResponse = await _bookingService.UpdateBooking(request);
                return Updated(bookingResponse);
            }
            catch (Exception)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }
    }
}
