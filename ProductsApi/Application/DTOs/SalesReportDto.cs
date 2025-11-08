public class SalesReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}
