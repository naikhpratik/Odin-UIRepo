using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class Notification : MobileTable
    {
        public NotificationType NotificationType { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

    }
}
