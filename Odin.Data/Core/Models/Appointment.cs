using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Appointment : MobileTable
    {
        public DateTime? ScheduledDate { get; set; }
        public string Description { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
