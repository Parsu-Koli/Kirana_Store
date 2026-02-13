using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Invoice Number is required")]
        [StringLength(20, ErrorMessage = "Invoice Number cannot exceed 20 characters")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Total Amount cannot be negative")]
        public decimal TotalAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount cannot be negative")]
        public decimal Discount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Net Amount cannot be negative")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "Payment Mode is required")]
        [RegularExpression("Cash|UPI|Card", ErrorMessage = "Payment Mode must be Cash, UPI, or Card")]
        public string PaymentMode { get; set; }

        [Required(ErrorMessage = "Sale Date is required")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Sale), nameof(ValidateSaleDate))]
        public DateTime SaleDate { get; set; }

        public List<SaleItem>? SaleItems { get; set; }

        public static ValidationResult? ValidateSaleDate(DateTime saleDate, ValidationContext context)
        {
            if (saleDate > DateTime.Now)
                return new ValidationResult("Sale Date cannot be in the future");
            return ValidationResult.Success;
        }
    }
}