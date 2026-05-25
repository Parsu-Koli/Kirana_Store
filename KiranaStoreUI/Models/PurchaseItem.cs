using System.ComponentModel.DataAnnotations;

namespace KiranaStoreUI.Models
{
    public class PurchaseItem
    {
        [Key]
        public int PurchaseItemId { get; set; }

        [Required(ErrorMessage = "PurchaseId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "PurchaseId must be greater than 0")]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "ProductId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Total is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total cannot be negative")]
        public decimal Total { get; set; }

        
    }
}
