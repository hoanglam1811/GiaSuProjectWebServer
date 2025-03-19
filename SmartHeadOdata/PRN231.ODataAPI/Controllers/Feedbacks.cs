using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers.OData
{
    public class Feedbacks : ODataController
    {
        private readonly IGenericService<Feedback, FeedbackDTO> _feedbackService;

        public Feedbacks(IGenericService<Feedback, FeedbackDTO> feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Feedback>>> Get()
        {
            var feedbackList = await _feedbackService.GetAll();
            return Ok(feedbackList);
        }

        [EnableQuery]
        public async Task<ActionResult<Feedback>> Get(int key)
        {
            var feedback = await _feedbackService.Get(key);
            return feedback != null ? Ok(feedback) : NotFound();
        }

        public async Task<ActionResult<Feedback>> Post([FromBody] FeedbackDTO dto)
        {
            var feedback = await _feedbackService.Add(dto);
            return Created(feedback);
        }

        public async Task<IActionResult> Put(int key, [FromBody] FeedbackDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var feedback = await _feedbackService.Update(dto);
            return Updated(feedback);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var feedback = await _feedbackService.Get(key);
            if (feedback == null) return NotFound();

            await _feedbackService.Delete(key);
            return NoContent();
        }
    }
}
