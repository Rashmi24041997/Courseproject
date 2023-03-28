using Courseproject.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Courseproject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DownloadController : ControllerBase
{
    //private IFileService FileService { get; }
    private IUploadService UploadService { get; }

	public DownloadController(/*IFileService fileService*/ IUploadService uploadService)
	{
        UploadService = uploadService;
        //FileService = fileService;
    }


    [HttpGet]
    [Route("Get/{path}")]
    public async Task<IActionResult> GetFile(string path)
    {
        //var bytes = FileService.GetFile(path);
        var bytes = await UploadService.GetFileAsync(path);
        return File(bytes, "APPLICATION/octet-stream", path);
    }

}
