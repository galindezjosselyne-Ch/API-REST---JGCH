using Microsoft.EntityFrameworkCore;
using ProductsApi.Application.Interfaces;
using ProductsApi.Application.Services;
using ProductsApi.Infrastructure.Persistence;
using ProductsApi.Infrastructure.Security;
using ProductsApi.Infrastructure.Services;

namespace ProductsApi.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("ProductsDb"));
        services.AddSingleton(new JwtTokenGenerator(config));
        services.AddSingleton<IStorageService, LocalStorageService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISalesService, SalesService>();

        return services;
    }
}
