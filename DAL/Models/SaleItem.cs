using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    public class SaleItem
    {
        [Key]
        public int SaleItemId { get; set; }
        public int SaleId { get; set; }        
        public int ProductId { get; set; }    
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }    
        public decimal Total { get; set; }

        public decimal GstPercentage { get; set; }

        public decimal GstAmount { get; set; }


        [JsonIgnore]
        [ForeignKey("SaleId")]
        public Sale? Sale { get; set; }

        [JsonIgnore]
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}



