using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Transferee : ApplicationUser
    {
        public Transferee()
        {
            LockoutEnabled = true;
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
