using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncomeTaxManagementSystem.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Application Number is required.")]
        [StringLength(50, ErrorMessage = "Application Number cannot exceed 50 characters.")]
        [Display(Name = "Application Number")]
        public string ApplicationNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Draft";

        [Display(Name = "Submitted Date")]
        public DateTime? SubmittedDate { get; set; }

        [Display(Name = "Reviewed Date")]
        public DateTime? ReviewedDate { get; set; }

        [StringLength(500, ErrorMessage = "Admin Remarks cannot exceed 500 characters.")]
        [Display(Name = "Admin Remarks")]
        public string? AdminRemarks { get; set; }
    }
}
