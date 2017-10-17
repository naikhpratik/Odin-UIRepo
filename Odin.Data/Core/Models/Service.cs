using System;
using Microsoft.Azure.Mobile.Server;

namespace Odin.Data.Core.Models
{
    public class Service : EntityData
    {
        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

        public int ServiceTypeId { get; set; }

        public ServiceType ServiceType {get; set;}

    }
}
