using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiranaStoreUI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }

        [BindNever]
        [NotMapped]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters")]
        public string ProductName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Unit is required")]
        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Purchase Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Purchase Price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "Selling Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Selling Price cannot be negative")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock cannot be negative")]
        public decimal QuantityInStock { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ExpiryDate { get; set; }

        public bool Active { get; set; } = true;

        [MaxLength(50)]
        public string? Barcode { get; set; }
    }
}
