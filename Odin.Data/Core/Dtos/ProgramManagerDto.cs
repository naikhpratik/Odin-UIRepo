using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class ProgramManagerDto
    {
        [Required]
        public int SeContactUid { get; set; }
    }
}
