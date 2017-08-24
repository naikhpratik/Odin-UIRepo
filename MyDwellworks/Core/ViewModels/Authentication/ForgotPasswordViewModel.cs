using System.ComponentModel.DataAnnotations;

namespace MyDwellworks.Core.ViewModels.Authentication
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}