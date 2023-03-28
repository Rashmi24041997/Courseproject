using Microsoft.AspNetCore.Http;

namespace Courseproject.Common.Interfaces;

public interface IUploadService
{
    Task DeleteFileAsync(string filePath);
    Task<byte[]> GetFileAsync(string filePath);
    Task<string> UploadFileAsync(IFormFile formFile);
}
