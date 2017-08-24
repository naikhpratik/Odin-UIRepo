using System.ComponentModel.DataAnnotations;

namespace MyDwellworks.Core.ViewModels.Authentication
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}