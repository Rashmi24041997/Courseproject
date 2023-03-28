using Microsoft.AspNetCore.Http;

namespace Courseproject.Common.Interfaces;

public interface IFileService
{
    void DeleteFile(string filePath);
    Task<string> SaveFileAsync(IFormFile file);
    byte[] GetFile(string filePath);
}
