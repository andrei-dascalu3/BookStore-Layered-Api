using BookStore.Presentation.Models;

namespace BookStore.Presentation.Interfaces;

public interface IFilesService
{
    Task<string> UploadAsync(IFormFile file);
    FileDownloadResult? GetFile(string fileName);
}
