using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers.OData
{
    public class SubjectODataController : ODataController
    {
        private readonly IGenericService<Subject, SubjectDTO> _subjectService;

        public SubjectODataController(IGenericService<Subject, SubjectDTO> subjectService)
        {
            _subjectService = subjectService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Subject>>> Get()
        {
            var subjectList = await _subjectService.GetAll();
            return Ok(subjectList);
        }

        [EnableQuery]
        public async Task<ActionResult<Subject>> Get(int key)
        {
            var subject = await _subjectService.Get(key);
            return subject != null ? Ok(subject) : NotFound();
        }

        public async Task<ActionResult<Subject>> Post([FromBody] SubjectDTO dto)
        {
            var subject = await _subjectService.Add(dto);
            return Created(subject);
        }

       public async Task<IActionResult> Put(int key, [FromBody] SubjectDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var subject = await _subjectService.Update(dto);
            return Updated(subject);
        }

       public async Task<IActionResult> Delete(int key)
        {
            var subject = await _subjectService.Get(key);
            if (subject == null) return NotFound();

            await _subjectService.Delete(key);
            return NoContent();
        }
    }
}
