using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Validations;

namespace Odin.Data.Core.Dtos
{
    public class ManagerDto
    {
        [Required]
        public int SeContactUid { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Email { get; set; }

        // Look at user roles, should be either Program Manager or Global Supply Chain
        [Required]
        [HasManagerRole(ErrorMessage = "Role must be Program Manager or Global Supply Chain")]
        public string Role { get; set; }
    }
}
