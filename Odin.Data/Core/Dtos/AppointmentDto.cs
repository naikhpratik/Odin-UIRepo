using System;

namespace Odin.Data.Core.Dtos
{
    public class AppointmentDto
    {
        public string Id { get; set; }

        public string OrderId { get; set; }

        public bool Deleted { get; set; }

        public string Description { get; set; }

        public DateTime? ScheduledDate { get; set; }      

    }
}
