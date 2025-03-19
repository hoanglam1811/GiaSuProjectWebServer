using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repositories.Interfaces;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Net;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IBookingRepository _bookingRepo;
        private readonly IBookingUserRepository _bookingUserRepository;
        private readonly IGenericService<User, UserDTO> _userService;

        public BookingController(IBookingService bookingService, IBookingUserRepository bookingUserRepository,
            IGenericService<User, UserDTO> userService,
            IBookingRepository bookingRepo)
        {
            _bookingService = bookingService;
            _bookingUserRepository = bookingUserRepository;
            _userService = userService;
            _bookingRepo = bookingRepo;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse>> GetAllBookings()
        {
            IEnumerable<BookingDto> bookings = await _bookingService.GetAllBookings();

            var response = new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookings);

            return Ok(response);
        }

        //[HttpGet("Get")]
        //public async Task<ActionResult<ApiResponse>> GetBookingByBookingId([FromRoute] int bookingId)
        //{
        //    Booking booking = await _bookingService.GetBooking(bookingId);

        //    var response = new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, booking);

        //    return Ok(response);
        //}

        [HttpGet("GetAllByStatus")]
        public async Task<ActionResult<ApiResponse>> GetAllBookingsByStatus([FromQuery] string status)
        {
            IEnumerable<BookingDto> bookings = await _bookingService.GetAllBookingsByStatus(status);

            return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookings));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            try
            {
                CreateBookingResponse bookingResponse = await _bookingService.CreateBooking(request);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse));
            } catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<ApiResponse>> UpdateBooking([FromBody] UpdateBookingRequest request)
        {
            try
            {
                UpdateBookingResponse bookingResponse = await _bookingService.UpdateBooking(request);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPut("UpdateStatus")]
        public async Task<ActionResult<ApiResponse>> UpdateBookingStatus([FromBody] UpdateBookingStatusRequest request)
        {
            try
            {
                var booking = await _bookingService.GetBooking(request.BookingId);
                booking.Status = request.Status;
                booking = await _bookingRepo.Update(booking);
                return Ok(booking);
                //UpdateBookingResponse bookingResponse = await _bookingService.UpdateBooking(booking);

                //return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, bookingResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("Apply")]
        public async Task<ActionResult<ApiResponse>> ApplyToBooking([FromBody] ApplyBookingRequest request)
        {
            var requests = await _bookingUserRepository.GetAll();
            requests = requests.Where(x => x.UserId == request.UserId && x.BookingId == request.BookingId);
            if(requests.Any()) {
                return BadRequest("Request existed!");
            }
            bool success = await _bookingService.ApplyToBooking(request.UserId, request.BookingId);

            if (success)
            {
                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, null));
            }
            else
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpGet("GetTutorsByBooking/{bookingId}")]
        public async Task<ActionResult<ApiResponse>> GetAllTutorsByBooking(int bookingId)
        {
            var tutors = await _bookingService.GetAllTutorsByBooking(bookingId);
            var response = new List<Object>();
            foreach (var tutor in tutors)
            {
                var users = await _userService.Get(tutor.UserId);
                var tutorInfo = new
                {
                    UserId = tutor.UserId,
                    BookingId = tutor.BookingId,
                    Role = tutor.Role,
                    Status = tutor.Status,
                    Description = tutor.Description,
                    User = users
                };
                response.Add(tutorInfo);
            }

            return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, response));
        }

        [HttpPost("AcceptTutor")]
        public async Task<ActionResult<ApiResponse>> AcceptTutor([FromBody] AcceptTutorRequest request)
        {
            var requests = await _bookingService.GetAllBookingsByStatus(BookingStatusConstant.APPROVED);
            var acceptedRequest = requests.FirstOrDefault(r => r.Id == request.BookingId);

            if (acceptedRequest != null)
            {
                return BadRequest("Request is already approved!");
            }

            bool success = await _bookingService.AcceptTutor(request.BookingId, request.TutorId);

            if (success)
            {
                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, null));
            }
            else
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpPost("CancelApplication")]
        public async Task<ActionResult<ApiResponse>> CancelApplication([FromBody] CancelApplicationRequest request)
        {
            bool success = await _bookingService.CancelApplication(request.UserId, request.BookingId);

            if (success)
            {
                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, null));
            }
            else
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }
    }
}