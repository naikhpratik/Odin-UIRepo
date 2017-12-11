using Newtonsoft.Json;

namespace Odin.PropBotWebJob.Dtos.Trulia
{
    public class TruliaBuyLocationDto
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

        [JsonProperty("zip")]
        public string PostalCode { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }
    }
}
