using Microsoft.AspNetCore.Http;

namespace backend.BLL.Services.Interfaces;

public interface IFileService
{
    Task<string> SaveFile(IFormFile file, string folder);
}