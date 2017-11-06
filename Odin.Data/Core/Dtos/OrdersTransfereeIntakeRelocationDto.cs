using System;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeRelocationDto
    {

        public string Id { get; set; }

        public DateTime? PreTripDate { get; set; }
       
        public string PreTripNotes { get; set; }
        
        public DateTime? EstimatedArrivalDate { get; set; }
       
        public DateTime? WorkStartDate { get; set; }
        
        public DateTime? EstimatedDepartureDate { get; set; }
    }
}
