using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services.Interfaces;
using System.Linq;

namespace PRN231.API.Controllers.OData
{
    public class LevelODataController : ODataController
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;
        private readonly IGenericRepository<Level> _levelRepo;

        public LevelODataController(
            IGenericService<Level, LevelDTO> levelService,
            IGenericRepository<Level> levelRepo)
        {
            _levelService = levelService;
            _levelRepo = levelRepo;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Level>>> Get()
        {
            var levelList = await _levelService.GetAll();
            return Ok(levelList);
        }

        [EnableQuery]
        public async Task<ActionResult<Level>> Get(int key)
        {
            var level = await _levelService.Get(key);
            return level != null ? Ok(level) : NotFound();
        }

        public async Task<ActionResult<Level>> Post([FromBody] LevelDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHandler.GetErrorMessage(ModelState));
            }

            try
            {
                var level = await _levelService.Add(dto);
                return Created(level);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Put(int key, [FromBody] LevelDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorHandler.GetErrorMessage(ModelState));
            }

            if (key != dto.Id) return BadRequest();

            try
            {
                var existingLevels = await _levelService.GetAll();
                existingLevels = existingLevels.Where(e => e.LevelName == dto.LevelName && e.Id != dto.Id);
                if (existingLevels.Any())
                {
                    return BadRequest("This name is existed");
                }

                var level = await _levelRepo.Get(dto.Id);
                if (level == null) return NotFound();

                level.LevelName = dto.LevelName;
                level.Status = dto.Status;
                await _levelRepo.Update(level);

                return Updated(level);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int key)
        {
            var level = await _levelService.Get(key);
            if (level == null) return NotFound();

            await _levelService.Delete(key);
            return NoContent();
        }
    }
}

