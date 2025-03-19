using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;

namespace PRN231.ODataAPI.Controllers
{
    public class LevelsController : ODataController
    {
        private readonly IGenericService<Level, LevelDTO> _levelService;

        public LevelsController(IGenericService<Level, LevelDTO> levelService)
        {
            _levelService = levelService;
        }

        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var allLevels = await _levelService.GetAll();

            return Ok(allLevels);
        }

        [EnableQuery]
        public async Task<IActionResult> Get(int key)
        {
            var level = await _levelService.Get(key);

            if(level  == null)
            {
                return NotFound();
            }

            return Ok(level);
        }

        public async Task<IActionResult> Post([FromBody] LevelDTO dto)
        {
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
            try
            {
                var existingLevel = await _levelService.Update(dto);

                if(existingLevel == null)
                {
                    return NotFound();
                }

                return Updated(existingLevel);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(int key)
        {
            try
            {
                var deletedLevel =  await _levelService.Delete(key);

                if(deletedLevel == null)
                {
                    return NotFound();
                }

                return Ok(deletedLevel);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
