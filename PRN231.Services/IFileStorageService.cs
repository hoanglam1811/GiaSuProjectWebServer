using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PRN231.Services;
public interface IFileStorageService
{
    Task<string> StoreFileAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string fileName);
    Task<IEnumerable<string>> GetAllFilesAsync();
}
