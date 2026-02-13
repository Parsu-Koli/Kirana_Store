//using System.Text.Json.Serialization;

//namespace DAL.Models
//{
//    public class OrderItem
//    {
//        public int OrderItemId { get; set; }    // Primary Key

//        public int OrderId { get; set; }        // FK to Order
//        public int ProductId { get; set; }

//        public int Quantity { get; set; }
//        public decimal Price { get; set; }      // Product Price
//        public decimal Total { get; set; }      // Quantity * Price

//        // Navigation (safe)
//        [JsonIgnore]
//        public Order? Order { get; set; }
//        [JsonIgnore]
//        public Product? Product { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }    // Primary Key

        [Required(ErrorMessage = "Order ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order ID must be greater than 0")]
        public int OrderId { get; set; }        // FK to Order

        [Required(ErrorMessage = "Product ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be greater than 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }      // Product Price

        [Required(ErrorMessage = "Total is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0")]
        public decimal Total { get; set; }      // Quantity * Price

        [JsonIgnore]
        public Order? Order { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
