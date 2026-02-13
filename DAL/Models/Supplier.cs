//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL.Models
//{
//    public class Supplier
//    {
//        public int SupplierId { get; set; }
//        public string SupplierName { get; set; }
//        public string Phone { get; set; }
//        public string Address { get; set; }
//    }

//}


using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Supplier Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Supplier Name must be between 3 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Supplier Name can contain letters and spaces only")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 250 characters")]
        public string Address { get; set; }
    }
}
