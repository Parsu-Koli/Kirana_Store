//namespace DAL.Models
//{
//    public class Purchase
//    {
//        public int PurchaseId { get; set; }
//        public int SupplierId { get; set; }
//        public DateTime PurchaseDate { get; set; }
//        public decimal TotalAmount { get; set; }
//    }

//}


using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be greater than 0")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Purchase date is required")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative")]
        public decimal TotalAmount { get; set; }
    }
}
