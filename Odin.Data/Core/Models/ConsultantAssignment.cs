using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class ConsultantAssignment
    {

        public Order Order { get; set; }
        public ApplicationUser Consultant { get; set; }
     
        [Key]
        [Column(Order=1)]
        public int OrderId { get; set; }
        
        [Key]
        [Column(Order = 2)]
        public string ConsultantId { get; set; }
    }
}
