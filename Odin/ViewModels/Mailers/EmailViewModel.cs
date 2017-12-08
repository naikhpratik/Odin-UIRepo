using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Odin.ViewModels.Mailers
{
    public class EmailViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        public string attachment { get; set; }
        public byte[] attBytes { get; set; }
        public string id { get; set; }
    }
}