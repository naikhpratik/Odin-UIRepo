using Newtonsoft.Json;
using Odin.Data.Helpers;
using System.Data.Entity.Spatial;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaRentDto
    {
        [JsonProperty("streetNumber")]
        public string StreetNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        public string Street1
        {
            get { return StreetNumber + ' ' + Street; }
        }

        [JsonProperty("apartmentNumber")]
        public string Street2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateCode")]
        public string State { get; set; }

        [JsonProperty("zipCode")]
        public string PostalCode { get; set; }

        [JsonProperty("numBedrooms")]
        public int? NumberOfBedrooms { get; set; }

        [JsonProperty("numBathrooms")]
        public decimal? NumberOfBathrooms { get; set; }

        [JsonProperty("sqft")]
        public int? SquareFootage { get; set; }

        [JsonProperty("price")]
        public decimal? Amount { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }

        public DbGeography Coordinate
        {
            get
            {
                if (Latitude.HasValue && Longitude.HasValue)
                {
                    return GeographyHelper.CreateCoordinate(Latitude.Value.ToString(), Longitude.Value.ToString());
                }
                return null;
            }
        }


    }
}
