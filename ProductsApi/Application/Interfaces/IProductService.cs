using ProductsApi.Application.DTOs;

namespace ProductsApi.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(string name, string description, decimal price, Stream? imageStream, string? imageName);
    Task<ProductDto?> UpdateAsync(int id, string? name, string? description, decimal? price, Stream? imageStream, string? imageName);
    Task<bool> DeleteAsync(int id);

}
