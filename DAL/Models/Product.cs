using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace DAL.Models
{
    public class Product
    {
        public required int ProductId { get; set; }
        public required int CategoryId { get; set; }

        [NotMapped]
        public string? CategoryName { get; set; }
        public required string ProductName { get; set; }
        public required string Description { get; set; }
        public required string Unit { get; set; }         
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        public required decimal QuantityInStock { get; set; }
        public required bool Active { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }
    }

}


//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text.Json.Serialization;
//namespace DAL.Models
//{
//    public class Product
//    {
//        [Key]
//        public int ProductId { get; set; }

//        [Required(ErrorMessage = "Category is required")]
//        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
//        public int CategoryId { get; set; }

//        [NotMapped]
//        public string? CategoryName { get; set; }

//        [Required(ErrorMessage = "Product Name is required")]
//        [StringLength(100, ErrorMessage = "Product Name cannot exceed 100 characters")]
//        public string ProductName { get; set; }

//        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
//        public string Description { get; set; }

//        [Required(ErrorMessage = "Unit is required")]
//        [StringLength(50, ErrorMessage = "Unit cannot exceed 50 characters")]
//        public string Unit { get; set; }

//        [Required(ErrorMessage = "Purchase Price is required")]
//        [Range(0, double.MaxValue, ErrorMessage = "Purchase Price cannot be negative")]
//        [Column(TypeName = "decimal(18,2)")]
//        public decimal PurchasePrice { get; set; }

//        [Required(ErrorMessage = "Selling Price is required")]
//        [Range(0, double.MaxValue, ErrorMessage = "Selling Price cannot be negative")]
//        [Column(TypeName = "decimal(18,2)")]
//        public decimal SellingPrice { get; set; }

//        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock cannot be negative")]
//        public decimal QuantityInStock { get; set; }

//        public bool Active { get; set; } = true;

//        [JsonIgnore]
//        public Category? Category { get; set; }
//    }
//}
