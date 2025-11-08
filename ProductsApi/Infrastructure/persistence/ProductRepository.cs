using Microsoft.EntityFrameworkCore;
using ProductsApi.Application.Interfaces;
using ProductsApi.Domain.Entities;

namespace ProductsApi.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _db.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(int id) => await _db.Products.FindAsync(id);

    public async Task<Product> AddAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
    }
}
