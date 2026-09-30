using System.ComponentModel.DataAnnotations;

namespace IncomeTaxManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "PAN is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN must be exactly 10 characters.")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN format. Example: ABCDE1234F")]
        [Display(Name = "PAN Number")]
        public string PAN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [StringLength(15, ErrorMessage = "Mobile number cannot exceed 15 digits.")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid mobile number. Please enter a valid 10-digit Indian mobile number.")]
        [Phone]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "User";

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<TaxDetails> TaxDetails { get; set; } = new List<TaxDetails>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
