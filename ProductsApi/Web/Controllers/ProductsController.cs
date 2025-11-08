using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Application.DTOs;
using ProductsApi.Application.Interfaces;

namespace ProductsApi.Web.Controllers;

[ApiController]
[Route("products")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ISalesService _salesService;

    public ProductsController(IProductService service, ISalesService salesService)
    {
        _service = service;
        _salesService = salesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var p = await _service.GetByIdAsync(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductForm form)
    {
        var image = form.Image?.OpenReadStream();
        var result = await _service.CreateAsync(form.Name, form.Description, form.Price, image, form.Image?.FileName);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductForm form)
    {
        var image = form.Image?.OpenReadStream();
        var result = await _service.UpdateAsync(id, form.Name, form.Description, form.Price, image, form.Image?.FileName);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("sales")]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleDto dto)
    {
        try
        {
            var sale = await _salesService.CreateSaleAsync(dto.ProductId, dto.Quantity);
            return Ok(new
            {
                sale.Id,
                sale.ProductId,
                ProductName = sale.Product.Name,
                sale.Quantity,
                sale.TotalPrice,
                sale.Date
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    [HttpGet("sales-report")]
    public async Task<IActionResult> GetSalesReport([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        if (start > end) return BadRequest("Rango de fechas inválido");

        var report = await _salesService.GetSalesReportAsync(start, end);

        var csv = new StringBuilder();
        csv.AppendLine("ProductId;ProductName;TotalQuantity;TotalRevenue");

        foreach (var r in report)
        {
            csv.AppendLine($"{r.ProductId};{r.ProductName};{r.TotalQuantity};{r.TotalRevenue}");
        }

        return File(
            Encoding.UTF8.GetBytes(csv.ToString()),
            "text/csv",
            $"sales-report-{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
        );
    }

}

public class ProductForm
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }
}
