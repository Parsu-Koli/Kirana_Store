//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL.Models
//{
//    public class Category
//    {
//        public int CategoryId { get; set; }
//        public string CategoryName { get; set; }
//    }
//}


using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Category
    {
        
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Category name can only contain letters and spaces")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}