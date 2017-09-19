using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Consultant : ApplicationUser
    {
        public virtual ICollection<Order> Orders { get; set; }
    }
}
