//namespace KiranaStoreUI.Models
//{
//    public class Category
//    {
//        public int CategoryId { get; set; }
//        public string CategoryName { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiranaStoreUI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Category name can only contain letters and spaces")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; }

        [NotMapped]
        public ICollection<Product>? Products { get; set; }
    }
}
