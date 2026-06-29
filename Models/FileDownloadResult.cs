namespace BookStore.Presentation.Models;

public class FileDownloadResult(string filePath, string contentType, string fileName)
{
    public string FilePath { get; } = filePath;
    public string ContentType { get; } = contentType;
    public string FileName { get; } = fileName;
}
