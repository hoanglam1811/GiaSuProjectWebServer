using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;
using PRN231.Services;
using PRN231.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using PRN231.Constant;

namespace PRN231.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CredentialController : ControllerBase
    {
        private readonly IGenericService<Credential, CredentialDTO> _credentialService;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<BookingUser> _bookingUserRepo;
        private readonly IGenericRepository<Credential> _credentialRepo;
        private readonly ILogger<CredentialController> _logger;
        public IConfiguration _configuration;
        private readonly IFileStorageService _fileStorageService;


        public CredentialController(IConfiguration config, ILogger<CredentialController> logger,
                IGenericRepository<Credential> credentialRepo,
                IGenericService<Credential, CredentialDTO> credentialService,
                IFileStorageService fileStorageService,
                IGenericRepository<BookingUser> bookingUserRepo,
                IGenericRepository<User> userRepo)
        {
            _logger = logger;
            _configuration = config;
            _credentialService = credentialService;
            _credentialRepo = credentialRepo;
            _fileStorageService = fileStorageService;
            _userRepo = userRepo;
            _bookingUserRepo = bookingUserRepo;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromForm] CredentialImageDTO credentialImage)
        {
            //try
            //{
            var credential = await _credentialRepo.Get(credentialImage.CredentialId);
            Console.WriteLine(credential);


            // Delete old file if exists
            if (!string.IsNullOrEmpty(credential.Image) && credential.Image.Contains("http://localhost:5176"))
                {
                    await _fileStorageService.DeleteFileAsync(Path.GetFileName(credential.Image));
                }

                // Store file
                string filePath = await _fileStorageService.StoreFileAsync(credentialImage.File);
                if (filePath == null)
                {
                    return BadRequest("Failed to store file.");
                }

                // Update credential image
                credential.Image = $"http://localhost:5176/{filePath}";
                var updatedCredential = await _credentialRepo.Update(credential);

                return Ok(updatedCredential);
            
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            //}
        }


        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var credentialList = await _credentialRepo.GetAll(x => x.Include(a => a.Subject));
            return Ok(credentialList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var credential = await _credentialService.Get(id);
            return Ok(credential);
        }

        [HttpGet("GetByUserId")]
        //[Authorize]
        public async Task<IActionResult> GetCredentialsByUserId(int id)
        {
            var credential = await _credentialRepo.GetAll(x => x.Include(a => a.Subject));
            credential = credential.Where(c => c.TutorId == id);
            return Ok(credential);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(CredentialDTO dto)
        {
            var credential = await _credentialService.Add(dto);
            return Ok(credential);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(CredentialDTO dto)
        {
            var bookingUserList = await _bookingUserRepo.GetAll(x => x
                    .Include(a => a.Booking)
                    .ThenInclude(b => b.Subject));
            bookingUserList = bookingUserList.Where(c => 
                    c.Booking.SubjectId == dto.SubjectId && c.Role == "TUTOR");
            if(bookingUserList.Count() != 0){
                return BadRequest(new {Message = "Credential already in use"});
            }
            dto.Status = StatusConstant.PENDING;
            var credential = await _credentialService.Update(dto);
            return Ok(credential);
        }

        [HttpPut("UpdateApprove")]
        public async Task<IActionResult> UpdateApprove([FromBody] CredentialDTO dto)
        {
            dto.Status = StatusConstant.ACTIVE;
            var credential = await _credentialService.Update(dto);

            return Ok(credential);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var credential = await _credentialService.Delete(id);
            return Ok(credential);
        }
    }
}
