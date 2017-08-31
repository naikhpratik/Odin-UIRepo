using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Transferee
    {
        public Transferee()
        {
            Orders = new Collection<Order>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpouseName { get; set; }

        public ICollection<Order> Orders { get; set; }
        
    }
}
