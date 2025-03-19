using Microsoft.AspNetCore.Mvc;
using PRN231.Models.DTOs;
using PRN231.Models;
using PRN231.Services.Interfaces;

namespace PRN231.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostRatingController : ControllerBase
    {
        private readonly IGenericService<PostRating, PostRatingDTO> _ratingService;
        private readonly IGenericService<Post, PostDTO> _postService;

        public PostRatingController(
            IGenericService<PostRating, PostRatingDTO> ratingService,
            IGenericService<Post, PostDTO> postService)
        {
            _ratingService = ratingService;
            _postService = postService;
        }

        [HttpPost("Rate")]
        public async Task<IActionResult> Rate([FromBody] PostRatingDTO dto)
        {
            var ratings = await _ratingService.GetAll(); 

            var existingRating = ratings.FirstOrDefault(r => r.PostId == dto.PostId && r.UserId == dto.UserId);
            if (existingRating != null)
            {
                return BadRequest("You can only rate this post once.");
            }

            var post = await _postService.Get(dto.PostId);
            if (post == null)
            {
                return NotFound($"Post with ID {dto.PostId} not found.");
            }

            var rating = await _ratingService.Add(dto);
            return Ok(rating);
        }


        [HttpGet("GetRatings")]
        public async Task<IActionResult> GetRatings(int postId)
        {
            var ratings = await _ratingService.GetAll();
            var postRatings = ratings.Where(r => r.PostId == postId).ToList();
            return Ok(postRatings);
        }

        [HttpGet("GetUserRating")]
        public async Task<IActionResult> GetUserRating(int postId, int userId)
        {
            var ratings = await _ratingService.GetAll();
            var userRating = ratings.FirstOrDefault(r => r.PostId == postId && r.UserId == userId);

            if (userRating == null)
            {
                return Ok();
            }

            return Ok(userRating);
        }

        [HttpGet("GetRatingsByUser")]
        public async Task<IActionResult> GetRatingsByUser(int userId)
        {
            var posts = await _postService.GetAll();
            var userPosts = posts.Where(p => p.UserId == userId).ToList();

            if (userPosts.Count == 0)
            {
                return Ok("No posts found for this user.");
            }

            var ratings = await _ratingService.GetAll();
            var userPostRatings = ratings.Where(r => userPosts.Any(p => p.Id == r.PostId)).ToList();

            return Ok(userPostRatings);
        }

    }

}
