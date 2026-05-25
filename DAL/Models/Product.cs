using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Date)]
        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ExpiryDate { get; set; }
        public required bool Active { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }
    }

}
