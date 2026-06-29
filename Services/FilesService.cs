using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Models;

namespace BookStore.Presentation.Services;

internal sealed class FilesService : IFilesService
{
    private readonly string _uploadsPath;
    private static readonly IDictionary<string, string> contentTypesByExtension = new Dictionary<string, string>()
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg"},
        { ".png", "image/png" },
        { ".pdf", "application/pdf" }
    };

    public FilesService(IWebHostEnvironment env)
    {
        _uploadsPath = Path.Combine(env.ContentRootPath, "uploads");
        Directory.CreateDirectory(_uploadsPath);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    public FileDownloadResult? GetFile(string fileName)
    {
        var filePath = Path.Combine(_uploadsPath, fileName);

        if (!File.Exists(filePath))
            return null;

        var contentType = contentTypesByExtension.TryGetValue(Path.GetExtension(fileName).ToLowerInvariant(), out var value) ? value : "application/octet-stream";

        return new FileDownloadResult(filePath, contentType, fileName);
    }
}
