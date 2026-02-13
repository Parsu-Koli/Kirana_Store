//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAL.Models
//{
//    public class User
//    {
//            public int UserId { get; set; }
//            public string FullName { get; set; }
//            public string Username { get; set; }
//            public string Password { get; set; }
//            public string Phone { get; set; }
//            public string Role { get; set; }      // Admin, Employee, Cashier
//            public bool IsActive { get; set; }
//            public DateTime CreatedDate { get; set; }


//    }
//}


using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full Name must be between 3 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name can contain letters and spaces only")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Username can contain letters, numbers, dots, underscores, and hyphens only")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [StringLength(20)]
        public string Role { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
