using System.ComponentModel.DataAnnotations;

namespace IncomeTaxManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email or PAN is required.")]
        [StringLength(100, ErrorMessage = "Email or PAN cannot exceed 100 characters.")]
        [Display(Name = "Email or PAN")]
        public string EmailOrPAN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
