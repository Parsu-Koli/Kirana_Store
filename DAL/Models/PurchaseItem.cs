//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL.Models
//{
//    public class PurchaseItem
//    {
//        public int PurchaseItemId { get; set; }
//        public int PurchaseId { get; set; }
//        public int ProductId { get; set; }
//        public int Quantity { get; set; }
//        public decimal Price { get; set; }
//        public decimal Total { get; set; }
//    }

//}


using System.ComponentModel.DataAnnotations;

namespace DAL.Models
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
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Total is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total cannot be negative")]
        public decimal Total { get; set; }


    }
}