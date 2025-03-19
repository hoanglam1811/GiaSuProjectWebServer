using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;
using PRN231.Models.DTOs;

namespace EXE101.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IGenericService<Role, RoleDTO> _roleService;
        private readonly ILogger<RoleController> _logger;
        public IConfiguration _configuration;

        public RoleController(IConfiguration config, ILogger<RoleController> logger,
                IGenericService<Role, RoleDTO> roleService)
        {
            _logger = logger;
            _configuration = config;
            _roleService= roleService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll(){
            var customerList = await _roleService.GetAll();
            return Ok(customerList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id){
            var customerList = await _roleService.Get(id);
            return Ok(customerList);
        }

        [HttpPost("Add")]
        //[Authorize]
        public async Task<IActionResult> Add(RoleDTO dto){
            var brand = await _roleService.Add(dto);
            return Ok(brand);
        }

        [HttpPut("Update")]
        //[Authorize]
        public async Task<IActionResult> Update(RoleDTO dto){
            var brand = await _roleService.Update(dto);
            return Ok(brand);
        }

        [HttpDelete("Delete")]
        //[Authorize]
        public async Task<IActionResult> Delete([FromBody]int id){
            var brand = await _roleService.Delete(id);
            return Ok(brand);
        }
    }
}
