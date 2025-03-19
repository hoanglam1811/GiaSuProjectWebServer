using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Repository.Interfaces;
using PRN231.Services;
using PRN231.Services.Interfaces;

namespace PRN231.API.Controllers.OData
{
    public class CredentialODataController : ODataController
    {
        private readonly IGenericService<Credential, CredentialDTO> _credentialService;
        private readonly IGenericRepository<Credential> _credentialRepo;
        private readonly IFileStorageService _fileStorageService;

        public CredentialODataController(
            IGenericRepository<Credential> credentialRepo,
            IGenericService<Credential, CredentialDTO> credentialService,
            IFileStorageService fileStorageService)
        {
            _credentialRepo = credentialRepo;
            _credentialService = credentialService;
            _fileStorageService = fileStorageService;
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Credential>>> Get()
        {
            var credentialList = await _credentialRepo.GetAll(x => x.Include(a => a.Subject));
            return Ok(credentialList);
        }

        [EnableQuery]
        public async Task<ActionResult<Credential>> Get(int key)
        {
            var credential = await _credentialService.Get(key);
            return credential != null ? Ok(credential) : NotFound();
        }

        public async Task<ActionResult<Credential>> Post([FromBody] CredentialDTO dto)
        {
            var credential = await _credentialService.Add(dto);
            return Created(credential);
        }

        public async Task<IActionResult> Put(int key, [FromBody] CredentialDTO dto)
        {
            if (key != dto.Id) return BadRequest();

            var credential = await _credentialService.Update(dto);
            return Updated(credential);
        }

        public async Task<IActionResult> Delete(int key)
        {
            var credential = await _credentialService.Get(key);
            if (credential == null) return NotFound();

            await _credentialService.Delete(key);
            return NoContent();
        }

        [EnableQuery]
        public async Task<ActionResult<IQueryable<Credential>>> GetByUserId(int id)
        {
            var credentials = await _credentialRepo.GetAll(x => x.Include(a => a.Subject));
            credentials = credentials.Where(c => c.TutorId == id);
            return Ok(credentials);
        }
    }
}
