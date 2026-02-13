using System.ComponentModel.DataAnnotations;
namespace DAL.Models
{
    public class StockLedger
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Transaction Type is required")]
        [RegularExpression("^(Purchase|Sale)$", ErrorMessage = "Transaction Type must be either 'Purchase' or 'Sale'")]
        public string TransactionType { get; set; }  // Purchase / Sale

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }

}
