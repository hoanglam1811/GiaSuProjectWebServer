using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using PRN231.Services;

namespace PRN231.API.Controllers.OData
{
    public class UsersController : ODataController
    {
        private readonly IGenericService<User, UserDTO> _userService;
        private readonly IGenericRepository<User> _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorageService;

        public UsersController(
            IGenericService<User, UserDTO> userService,
            IGenericRepository<User> userRepo,
            UserManager<User> userManager,
            IFileStorageService fileStorageService)
        {
            _userService = userService;
            _userRepo = userRepo;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<ResponseUserDTO>>> Get()
        {
            var users = await _userRepo.GetAll(x => x.Include(x => x.Credentials).Include(x => x.BookingUsers));
            var responseUserList = new List<ResponseUserDTO>();
            foreach (var user in users)
            {
                var role = await _userManager.GetRolesAsync(user);
                var userDto = new ResponseUserDTO
                {
                    // ... (Map properties from 'user' to 'userDto')
                };
                responseUserList.Add(userDto);
            }
            return Ok(responseUserList);
        }

        [EnableQuery]
        public async Task<ActionResult<User>> Get(int key)
        {
            var customerList = await _userRepo.GetAll(x => x.Include(x => x.Credentials));
            var customer = customerList.FirstOrDefault(x => x.Id == key);
            return Ok(customer);
        }

        public async Task<ActionResult<User>> Post([FromBody] UserDTO dto)
        {
            var user = await _userService.Add(dto);
            return Created(user);
        }

        public async Task<IActionResult> Put(int key, [FromBody] UserInfoDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var user = await _userService.Get(dto.Id);
            if (user == null) return NotFound();
            var updatedUser = await _userRepo.Update(user);
            return Updated(updatedUser);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var user = await _userService.Get(key);
            if (user == null) return NotFound();

            await _userService.Delete(key);
            return NoContent();
        }
    }
}
