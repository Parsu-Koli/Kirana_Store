//using System.ComponentModel.DataAnnotations;

//namespace DAL.Models
//{
//    public class Payment
//    {
//        public int PaymentId { get; set; }
//        public int SaleId { get; set; }
//        public decimal AmountPaid { get; set; }
//        public string Mode { get; set; }         
//        public DateTime PaymentDate { get; set; }
//    }

//}



using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Sale ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Sale ID must be greater than 0")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Amount Paid is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount Paid must be greater than 0")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment Mode is required")]
        [RegularExpression("Cash|UPI|Card", ErrorMessage = "Mode must be either Cash, UPI, or Card")]
        public string Mode { get; set; }         // UPI, Cash, Card

        [Required(ErrorMessage = "Payment Date is required")]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
    }
}
