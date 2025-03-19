using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers.OData
{
    public class RoleODataController : ODataController
    {
        private readonly IGenericService<Role, RoleDTO> _roleService;

        public RoleODataController(IGenericService<Role, RoleDTO> roleService)
        {
            _roleService = roleService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Role>>> Get()
        {
            var roleList = await _roleService.GetAll();
            return Ok(roleList);
        }

        [EnableQuery]
        public async Task<ActionResult<Role>> Get(int key)
        {
            var role = await _roleService.Get(key);
            return role != null ? Ok(role) : NotFound();
        }

        public async Task<ActionResult<Role>> Post([FromBody] RoleDTO dto)
        {
            var role = await _roleService.Add(dto);
            return Created(role);
        }

        public async Task<IActionResult> Put(int key, [FromBody] RoleDTO dto)
        {
            var role = await _roleService.Update(dto);
            return Updated(role);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var role = await _roleService.Get(key);
            if (role == null) return NotFound();

            await _roleService.Delete(key);
            return NoContent();
        }
    }
}
