using Newtonsoft.Json;

namespace Odin.Domain.Bots.Dtos
{
    public class TruliaRentDto
    {
        [JsonProperty("streetNumber")]
        public string StreetNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        public string PropertyStreet1
        {
            get { return StreetNumber + ' ' + Street; }
        }

        [JsonProperty("apartmentNumber")]
        public string PropertyStreet2 { get; set; }

        [JsonProperty("city")]
        public string PropertyCity { get; set; }

        [JsonProperty("stateCode")]
        public string PropertyState { get; set; }

        [JsonProperty("zipCode")]
        public string PropertyPostalCode { get; set; }

        [JsonProperty("numBedrooms")]
        public int? PropertyNumberOfBedrooms { get; set; }

        [JsonProperty("numBathrooms")]
        public decimal? PropertyNumberOfBathrooms { get; set; }

        [JsonProperty("sqft")]
        public int? PropertySquareFootage { get; set; }

        [JsonProperty("price")]
        public decimal? PropertyAmount { get; set; }

        [JsonProperty("latitude")]
        public decimal? PropertyLatitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? PropertyLongitude { get; set; }


    }
}
