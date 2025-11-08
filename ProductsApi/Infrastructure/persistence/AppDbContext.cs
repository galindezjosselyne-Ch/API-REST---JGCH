using Microsoft.EntityFrameworkCore;
using ProductsApi.Domain.Entities;

namespace ProductsApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Sale> Sales { get; set; } = null!;

}
