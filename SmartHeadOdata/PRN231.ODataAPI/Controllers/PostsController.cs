using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Services;
using PRN231.Services.Interfaces;
using System.Linq;

namespace PRN231.API.Controllers.OData
{
    public class PostODataController : ODataController
    {
        private readonly IGenericService<Post, PostDTO> _postService;
        private readonly IFileStorageService _fileStorageService;

        public PostODataController(
            IGenericService<Post, PostDTO> postService,
            IFileStorageService fileStorageService)
        {
            _postService = postService;
            _fileStorageService = fileStorageService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Post>>> Get()
        {
            var postList = await _postService.GetAll();
            return Ok(postList);
        }

        [EnableQuery]
        public async Task<ActionResult<Post>> Post([FromForm] PostDTO dto)
        {
            var imageUrls = new List<string>();
            if (dto.ImageFiles != null && dto.ImageFiles.Count > 0)
            {
                foreach (var file in dto.ImageFiles)
                {
                    var imageUrl = await _fileStorageService.StoreFileAsync(file);
                    if (imageUrl == null)
                    {
                        return BadRequest("Failed to store one or more files.");
                    }
                    imageUrl = $"http://localhost:5176/{imageUrl}";
                    imageUrls.Add(imageUrl);
                }
            }

            dto.ImageUrlList = imageUrls;
            dto.Status = StatusConstant.ACTIVE;

            var post = await _postService.Add(dto);
            return Created(post);
        }

        public async Task<IActionResult> Put(int key, [FromForm] PostDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var newImageUrls = new List<string>();
            if (dto.ImageFiles != null && dto.ImageFiles.Count > 0)
            {
                foreach (var file in dto.ImageFiles)
                {
                    var newImageUrl = await _fileStorageService.StoreFileAsync(file);
                    if (newImageUrl == null)
                    {
                        return BadRequest("Failed to store one or more files.");
                    }
                    newImageUrl = $"http://localhost:5176/{newImageUrl}";
                    newImageUrls.Add(newImageUrl);
                }

                if (dto.ImageUrl != null && dto.ImageUrlList.Count > 0)
                {
                    foreach (var oldImageUrl in dto.ImageUrlList)
                    {
                        await _fileStorageService.DeleteFileAsync(oldImageUrl);
                    }
                }

                dto.ImageUrlList = newImageUrls;
            }

            dto.Status = StatusConstant.ACTIVE;

            var updatedPost = await _postService.Update(dto);
            return Updated(updatedPost);
        }

    }
}
