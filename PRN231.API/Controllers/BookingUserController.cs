using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingUserController : ControllerBase
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;
        private readonly IGenericService<Subject, SubjectDTO> _subjectService;
        private readonly IGenericService<BookingUser, BookingUserDTO> _bookingUserService;
        private readonly IGenericRepository<BookingUser> _bookingUserRepo;
        private readonly IGenericRepository<Credential> _credentialRepo;
        private readonly ILogger<BookingUserController> _logger;

        public BookingUserController(ILogger<BookingUserController> logger,
                IGenericRepository<BookingUser> bookingUserRepo,
                IGenericService<BookingUser, BookingUserDTO> bookingUserService,
                IGenericService<Level, LevelDTO> levelService,
                IGenericService<Subject, SubjectDTO> subjectService,
                IGenericRepository<Credential> credentialRepo)
        {
            _logger = logger;
            _bookingUserRepo = bookingUserRepo;
            _bookingUserService = bookingUserService;
            _levelService = levelService;
            _subjectService = subjectService;
            _credentialRepo = credentialRepo;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var bookingUserList = await _bookingUserRepo.GetAll();
            return Ok(bookingUserList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var bookingUser = await _bookingUserService.Get(id);
            return Ok(bookingUser);
        }

        [HttpGet("GetBookingUserByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetBookingUserByUserId(int id)
        {
            var bookingUserList = await _bookingUserRepo.GetAll(x => x.Include(a => a.User));
            bookingUserList = bookingUserList.Where(c => c.UserId == id);
            return Ok(bookingUserList);
        }

        [HttpGet("GetCredentialStatusByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetCredentialStatusByUserId(int id)
        {
            
            var credentialList = await _credentialRepo.GetAll();
            credentialList = credentialList.Where(x => x.TutorId == id);
            var credentialDic = new Dictionary<int, string>();
            foreach(var credential in credentialList){
                var bookingUserList = await _bookingUserRepo.GetAll(x => x
                    .Include(a => a.Booking)
                    .ThenInclude(b => b.Subject));
                bookingUserList = bookingUserList.Where(c => 
                    c.Booking.SubjectId == credential.SubjectId && c.Role == "TUTOR");
                var status = bookingUserList.Count() == 0 ? "No Booking" : "Has Booking";
                credentialDic.Add(credential.Id, status);
            }
            return Ok(credentialDic);
        }


        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(BookingUserDTO dto)
        {
            var bookingUser = await _bookingUserService.Add(dto);
            return Ok(bookingUser);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(BookingUserDTO dto)
        {
            var bookingUser = await _bookingUserService.Update(dto);
            return Ok(bookingUser);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var bookingUser = await _bookingUserService.Delete(id);
            return Ok(bookingUser);
        }

        [HttpGet("GetBookingUserByUserIdFeedback")]
        public async Task<IActionResult> GetBookingUserByUserIdFeedback(int id)
        {
            var bookingUserList = await _bookingUserRepo.GetAll(x => x.Include(a => a.User), x => x.Include(a => a.Booking));
            var bookingIdList = bookingUserList.Where(c => c.UserId == id && c.Booking.Status == "DONE").Select(x => x.BookingId);
            List<BookingUser> bookingUsers = new List<BookingUser>();
            foreach (var BookingId in  bookingIdList)
            {
                var bookingUser = await _bookingUserRepo.GetAll();
                bookingUser = bookingUser.Where(b => b.BookingId == BookingId && b.Role == "TUTOR" && b.Status == "APPROVED");
                bookingUsers.AddRange(bookingUser);
            }

            foreach (var bookingUser in bookingUsers)
            {
                bookingUser.User.BookingUsers = null;
                bookingUser.Booking.BookingUsers = null;
                var subject = await _subjectService.Get(bookingUser.Booking.SubjectId);
                var level = await _levelService.Get(bookingUser.Booking.LevelId);
                bookingUser.Booking.Subject = new Subject();
                bookingUser.Booking.Level = new Level();
                bookingUser.Booking.Subject.Name = subject.Name;
                bookingUser.Booking.Level.LevelName = level.LevelName;
                
            }
            return Ok(bookingUsers);
            

            //return Ok(bookingUserList.Select(x => new BookingUser
            //{
            //    UserId = x.UserId,
            //    Role = x.Role,
            //    Status = x.Status,
            //    BookingId = x.BookingId,
            //    Description = x.Description,
            //    User = new User
            //    {
            //        Id = x.UserId,

            //    }
            //}));
        }
    }
}
