using System.ComponentModel.DataAnnotations;

namespace MyDwellworks.Core.ViewModels.Authentication
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}