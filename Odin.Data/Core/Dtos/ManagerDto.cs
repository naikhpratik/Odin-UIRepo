using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Role { get; set; }
    }
}
