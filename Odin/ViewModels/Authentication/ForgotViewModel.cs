﻿using System.ComponentModel.DataAnnotations;

namespace Odin.ViewModels.Authentication
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}