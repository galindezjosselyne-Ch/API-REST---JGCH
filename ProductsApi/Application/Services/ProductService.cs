using ProductsApi.Application.DTOs;
using ProductsApi.Application.Interfaces;
using ProductsApi.Domain.Entities;

namespace ProductsApi.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IStorageService _storage;

    public ProductService(IProductRepository repo, IStorageService storage)
    {
        _repo = repo;
        _storage = storage;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return products.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl));
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p is null ? null : new ProductDto(p.Id, p.Name, p.Description, p.Price, p.ImageUrl);
    }

    public async Task<ProductDto> CreateAsync(string name, string description, decimal price, Stream? imageStream, string? imageName)
    {
        string? imageUrl = null;

        if (imageStream != null && imageName != null)
        {
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(imagesPath);

            var safeFileName = Path.GetFileName(imageName);
            var filePath = Path.Combine(imagesPath, safeFileName);

            using var fs = new FileStream(filePath, FileMode.Create);
            await imageStream.CopyToAsync(fs);

            imageUrl = $"/images/{safeFileName}";
        }

        var product = new Product { Name = name, Description = description, Price = price, ImageUrl = imageUrl };
        await _repo.AddAsync(product);

        return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.ImageUrl);
    }

    public async Task<ProductDto?> UpdateAsync(int id, string? name, string? description, decimal? price, Stream? imageStream, string? imageName)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return null;

        if (!string.IsNullOrEmpty(name)) product.Name = name;
        if (!string.IsNullOrEmpty(description)) product.Description = description;
        if (price.HasValue) product.Price = price.Value;

        if (imageStream != null && imageName != null)
            product.ImageUrl = await _storage.UploadFileAsync(imageStream, imageName);

        await _repo.UpdateAsync(product);
        return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.ImageUrl);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product == null) return false;
        await _repo.DeleteAsync(product);
        return true;
    }
}
