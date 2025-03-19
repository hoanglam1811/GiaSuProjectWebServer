using Microsoft.AspNetCore.Http;

namespace PRN231.API.Controllers
{
    public class CredentialImageDTO
    {
        public int CredentialId { get; set; }
        public IFormFile File { get; set; }
    }
}
