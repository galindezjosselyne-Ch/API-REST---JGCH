
namespace ProductsApi.Application.Interfaces;

public interface ISalesService
{
    Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(DateTime start, DateTime end);
    Task<Sale> CreateSaleAsync(int productId, int quantity);
}
