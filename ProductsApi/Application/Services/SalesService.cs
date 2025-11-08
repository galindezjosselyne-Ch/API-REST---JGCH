using Microsoft.EntityFrameworkCore;
using ProductsApi.Application.DTOs;
using ProductsApi.Application.Interfaces;
using ProductsApi.Infrastructure.Persistence;

namespace ProductsApi.Infrastructure.Services;

public class SalesService : ISalesService
{
    private readonly AppDbContext _db;

    public SalesService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Sale> CreateSaleAsync(int productId, int quantity)
    {
        var product = await _db.Products.FindAsync(productId);
        if (product == null) throw new Exception("Producto no encontrado");

        var sale = new Sale
        {
            ProductId = productId,
            Product = product,
            Quantity = quantity,
            TotalPrice = product.Price * quantity,
            Date = DateTime.UtcNow
        };

        _db.Sales.Add(sale);
        await _db.SaveChangesAsync();

        return sale;
    }


    public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime start, DateTime end)
    {
        var sales = await _db.Sales
            .Include(s => s.Product)
            .Where(s => s.Date >= start && s.Date <= end)
            .ToListAsync();

        var report = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new SalesReportDto
            {
                ProductId = g.Key,
                ProductName = g.First().Product.Name,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.TotalPrice)
            });

        return report;
    }
}
