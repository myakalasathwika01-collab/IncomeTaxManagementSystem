using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncomeTaxManagementSystem.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Document Type is required.")]
        [StringLength(100, ErrorMessage = "Document Type cannot exceed 100 characters.")]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "File Name is required.")]
        [StringLength(255, ErrorMessage = "File Name cannot exceed 255 characters.")]
        [Display(Name = "File Name")]
        public string FileName { get; set; } = string.Empty;

        [Required(ErrorMessage = "File Path is required.")]
        [StringLength(500, ErrorMessage = "File Path cannot exceed 500 characters.")]
        [Display(Name = "File Path")]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}
