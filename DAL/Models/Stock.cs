using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }  // Primary Key

        [Required(ErrorMessage = "Product is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
        public int ProductId { get; set; }  // Foreign Key to Product

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
        public decimal Quantity { get; set; }  // Current stock count

        [Required(ErrorMessage = "Minimum Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum Quantity cannot be negative")]
        public decimal MinimumQuantity { get; set; }  // For low stock alert

        public Product Product { get; set; }
    }
}
