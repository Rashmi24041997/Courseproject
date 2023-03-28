using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Courseproject.Infrastructure;

public class FileService : IFileService
{
    private const string UPLOAD_DIRECTORY = "uploads";

    public FileService()
    {
        if(!Directory.Exists(UPLOAD_DIRECTORY))
            Directory.CreateDirectory(UPLOAD_DIRECTORY);
    }

    public void DeleteFile(string filePath)
    {
        var path = Path.Combine(UPLOAD_DIRECTORY, filePath);
        File.Delete(path);
    }

    public byte[] GetFile(string filePath)
    {
        var path = Path.Combine(UPLOAD_DIRECTORY, filePath);
        return File.ReadAllBytes(path);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var uniqueFilename = Guid.NewGuid().ToString() + "_" + file.FileName;
        var path = Path.Combine(UPLOAD_DIRECTORY, uniqueFilename);
        await file.CopyToAsync(new FileStream(path, FileMode.Create));
        return uniqueFilename;
    }
}
