using System;

namespace Odin.Data.Core.Models
{
    public class Service
    {
        public int Id { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ServiceTypeId { get; set; }

        public ServiceType ServiceType {get; set;}

    }
}
