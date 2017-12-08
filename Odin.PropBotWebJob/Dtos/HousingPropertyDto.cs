using System;

namespace Odin.PropBotWebJob.Dtos
{
    public class HousingPropertyDto
    {
        public String OrderId { get; set; }

        public String PropertyStreet1 { get; set; }

        public String PropertyStreet2 { get; set; }

        public String PropertyCity { get; set; }

        public String PropertyState { get; set; }

        public String PropertyPostalCode { get; set; }

        public DateTime? PropertyAvailabilityDate { get; set; }

        public int? PropertyNumberOfBedrooms { get; set; }

        public decimal? PropertyNumberOfBathrooms { get; set; }

        public int? PropertySquareFootage { get; set; }

        public decimal? PropertyAmount { get; set; }

        public decimal? PropertyLatitude { get; set; }

        public decimal? PropertyLongitude { get; set; }

        public string SourceUrl { get; set; }

        public string PropertyDescription { get; set; }
    }
}
