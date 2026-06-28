using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FilesController : ControllerBase
{
    private readonly string _uploadsPath;

    public FilesController(IWebHostEnvironment env)
    {
        _uploadsPath = Path.Combine(env.ContentRootPath, "uploads");
        Directory.CreateDirectory(_uploadsPath);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { fileName });
    }

    [AllowAnonymous]
    [HttpGet("{fileName}")]
    public IActionResult Download(string fileName)
    {
        var filePath = Path.Combine(_uploadsPath, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };

        return PhysicalFile(filePath, contentType, fileName);
    }
}
