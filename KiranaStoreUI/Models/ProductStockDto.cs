namespace KiranaStoreUI.Models
{
    public class ProductStockDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal QuantityInStock { get; set; }
        public decimal PurchasePrice { get; internal set; }
    }

    public class SalesOverviewDto
    {
        public string Date { get; set; }
        public decimal TotalSales { get; set; }
    }
    public class PaymentModeDto
    {
        public string PaymentMode { get; set; }
        public int Count { get; set; }
    }
}


