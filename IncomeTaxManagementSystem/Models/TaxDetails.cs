using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IncomeTaxManagementSystem.Models
{
    public class TaxDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required(ErrorMessage = "Assessment Year is required.")]
        [StringLength(20, ErrorMessage = "Assessment Year cannot exceed 20 characters.")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Assessment Year must be in format YYYY-YYYY (e.g., 2024-2025).")]
        [Display(Name = "Assessment Year")]
        public string AssessmentYear { get; set; } = string.Empty;

        [Required(ErrorMessage = "Employment Type is required.")]
        [StringLength(50, ErrorMessage = "Employment Type cannot exceed 50 characters.")]
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Employer Name cannot exceed 150 characters.")]
        [Display(Name = "Employer Name")]
        public string? EmployerName { get; set; }

        [Required(ErrorMessage = "Gross Salary is required.")]
        [Range(0, 999999999999.99, ErrorMessage = "Gross Salary must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Gross Salary")]
        public decimal GrossSalary { get; set; }

        [Range(0, 999999999999.99, ErrorMessage = "Other Income must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Other Income")]
        public decimal OtherIncome { get; set; }

        [Range(0, 999999999999.99, ErrorMessage = "TDS must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "TDS")]
        public decimal TDS { get; set; }

        [Range(0, 999999999999.99, ErrorMessage = "Deductions must be a positive value.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Deductions")]
        public decimal Deductions { get; set; }

        [Required(ErrorMessage = "Tax Regime is required.")]
        [StringLength(20, ErrorMessage = "Tax Regime cannot exceed 20 characters.")]
        [Display(Name = "Tax Regime")]
        public string TaxRegime { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Remarks cannot exceed 500 characters.")]
        public string? Remarks { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
