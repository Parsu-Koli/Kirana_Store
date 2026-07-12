using System.ComponentModel.DataAnnotations;

namespace KiranaStoreUI.Models
{
    public class GstRegistration
    {
        [Key]
        public int GstRegistrationId { get; set; }

        [Required]
        [MaxLength(15)]
        public string GstNumber { get; set; }

        [Required]
        [MaxLength(150)]
        public string BusinessName { get; set; }

        [MaxLength(100)]
        public string? OwnerName { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(10)]
        public string? StateCode { get; set; }

        [MaxLength(10)]
        public string? PinCode { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? MobileNumber { get; set; }

        public bool IsGstEnabled { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}