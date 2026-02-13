using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

}


//using System.ComponentModel.DataAnnotations;
//namespace DAL.Models
//{
//    public class Customer
//    {
//        [Key]
//        public int CustomerId { get; set; }

//        [Required(ErrorMessage = "Customer name is required")]
//        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
//        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
//        public string Name { get; set; }

//        [Required(ErrorMessage = "Phone number is required")]
//        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
//        public string Phone { get; set; }
//    }
