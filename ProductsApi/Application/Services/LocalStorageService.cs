using ProductsApi.Application.Interfaces;

public class LocalStorageService : IStorageService
{
    private readonly string _imagesPath;

    public LocalStorageService()
    {
        _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        Directory.CreateDirectory(_imagesPath);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var safeFileName = Path.GetFileName(fileName);
        var filePath = Path.Combine(_imagesPath, safeFileName);
        using var fs = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(fs);
        return $"/images/{safeFileName}";
    }
}
