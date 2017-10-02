using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Odin.Data.Core.Dtos
{
    public class ManagersDto
    {
        [Required]
        public List<ManagerDto> Managers { get; set; }
    }
}