using System;

namespace Odin.Data.Core.Dtos
{
    public class ServiceDto
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public bool Selected { get; set; }

        public string Notes { get; set; }

        public DateTime? ScheduledDate { get; set; }      

        public DateTime? CompletedDate { get; set; }
        
        public int ServiceTypeId { get; set; }
    }
}
