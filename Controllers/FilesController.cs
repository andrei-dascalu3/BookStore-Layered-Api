using BookStore.Presentation.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var fileName = await _filesService.UploadAsync(file);
        return Ok(new { fileName });
    }

    [AllowAnonymous]
    [HttpGet("{fileName}")]
    public IActionResult Download(string fileName)
    {
        var result = _filesService.GetFile(fileName);
        if (result == null)
        {
            return NotFound();
        }

        return PhysicalFile(result.FilePath, result.ContentType, result.FileName);
    }
}
